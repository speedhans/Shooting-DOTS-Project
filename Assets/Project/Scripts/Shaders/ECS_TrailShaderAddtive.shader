Shader "Custom/ECS_TrailShaderAddtive"
{
    Properties{
    _TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex("Particle Texture", 2D) = "white" {}
    _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    }

        Category{
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            Blend SrcAlpha One
            AlphaTest Greater .01
            ColorMask RGB
            Cull Off Lighting Off ZWrite Off Fog { Color(0,0,0,0) }
            BindChannels {
                Bind "Color", color
                Bind "Vertex", vertex
                Bind "TexCoord", texcoord
            }

        // ---- Fragment program cards
        SubShader {
            Pass {

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma multi_compile_particles
                #pragma multi_compile_instancing
                //#pragma instancing_options procedural:setup

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                fixed4 _TintColor;

                struct appdata {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 uv : TEXCOORD0;
                    #ifdef SOFTPARTICLES_ON
                    float4 projPos : TEXCOORD1;
                    #endif
                };

                float4 _MainTex_ST;

                float3 hsv2rgb(float3 c)
                {
                    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
                }

                // ComputeBufferを受け取る
                StructuredBuffer<float3> _Positions;
                StructuredBuffer<int3> _Segments;


                // 頂点を加工し、Trailの位置に移動させる
                void TransformVertex(inout float3 _vertex, uint _instanceID)
                {//, inout float4 _color : COLOR0) {
                    // Segment情報を取得
                    int index = _vertex.x;
                    int3 segment = _Segments[_instanceID];

                    // 今計算中のTrail、次のTrail、前のTrailの位置を求める
                    float3 p1 = _Positions[segment.x + index];
                    float3 p0 = index == 0 ? p1 + p1 - _Positions[segment.x + index + 1] : _Positions[segment.x + index - 1];
                    float3 p2 = index == segment.y - 1 ? p1 + p1 - _Positions[segment.x + index - 1] : _Positions[segment.x + index + 1];

                    // Trailの位置関係から、接空間を求める
                    float3 tangent = normalize(p2 - p0);
                    float3 binormal = cross(tangent, float3(0, 1, 0));
                    float3 normal = cross(tangent, binormal);

                    // 頂点位置を計算する
                    _vertex = p1 + 0.05 * (_vertex.y * binormal + _vertex.z * normal);

                    // 色を適当に計算する
                    //_color.rgb = hsv2rgb(float3(frac(segment.z * 0.1), 1, 1));
                    //_color.a = index < segment.y ? 1 : 0; // segmentのサイズ以上の頂点を描画中なら、alphaを0（非表示）にする
                }

                v2f vert(appdata v, uint instanceID : SV_InstanceID)
                {
                    v2f o;
                    //UNITY_SETUP_INSTANCE_ID(v);
                    TransformVertex(v.vertex.xyz, instanceID);// , o.color); // 頂点を加工する

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #ifdef SOFTPARTICLES_ON
                    o.projPos = ComputeScreenPos(o.vertex);
                    COMPUTE_EYEDEPTH(o.projPos.z);
                    #endif
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;
                }

                sampler2D _CameraDepthTexture;
                float _InvFade;

                fixed4 frag(v2f i) : COLOR
                {
                    #ifdef SOFTPARTICLES_ON
                    float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
                    float partZ = i.projPos.z;
                    float fade = saturate(_InvFade * (sceneZ - partZ));
                    i.color.a *= fade;
                    #endif

                    return 2.0f * i.color * _TintColor * tex2D(_MainTex, i.uv);
                }
                ENDCG
            }
        }

        // ---- Dual texture cards
        SubShader {
            Pass {
                SetTexture[_MainTex] {
                    constantColor[_TintColor]
                    combine constant * primary
                }
                SetTexture[_MainTex] {
                    combine texture * previous DOUBLE
                }
            }
        }

        // ---- Single texture cards (does not do color tint)
        SubShader {
            Pass {
                SetTexture[_MainTex] {
                    combine texture * primary
                }
            }
        }
    }
}
