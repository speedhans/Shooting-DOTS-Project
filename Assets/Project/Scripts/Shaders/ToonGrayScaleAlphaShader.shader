Shader "Custom/ToonGrayScaleAlphaShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTexture", 2D) = "white" {}
        _Color("DiffuseColor", Color) = (0.7169812, 0.7169812, 0.7169812, 1)
        _Specular("Specular", Color) = (0, 0, 0, 1)
        _Smoothness("Smoothness", Float) = 0.5
        [NoScaleOffset]_LightingRamp("LightingRamp", 2D) = "white" {}

    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent+0"
        }

        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

        // Render State
        Blend SrcAlpha OneMinusSrcAlpha//Blend One Zero, One Zero
        Cull Back
        ZTest LEqual
        ZWrite On
        // ColorMask: <None>


        HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        // Pragmas
        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma target 2.0
        #pragma multi_compile_fog
        #pragma multi_compile_instancing

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
        #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        // GraphKeywords: <None>

        // Defines
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS 
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_BITANGENT_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define SHADERPASS_FORWARD

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float4 _Specular;
        float _Smoothness;
        CBUFFER_END
        TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
        TEXTURE2D(_LightingRamp); SAMPLER(sampler_LightingRamp); float4 _LightingRamp_TexelSize;
        SAMPLER(SamplerState_Point_Clamp);
        SAMPLER(_SampleTexture2D_C5EF5AEE_Sampler_3_Linear_Repeat);


        uniform float GrayScaleFactor;
        uniform float3 GrayScaleColor;
        // Graph Functions

        // 10567835598322a437e189e9044dec2b
        #include "Assets/Project/Scripts/Shaders/Includes/CustomLighting.hlsl"

        struct Bindings_GetMainLight_52012c17518825a429793d26daee4a8c
        {
            float3 AbsoluteWorldSpacePosition;
        };

        void SG_GetMainLight_52012c17518825a429793d26daee4a8c(Bindings_GetMainLight_52012c17518825a429793d26daee4a8c IN, out half3 Direction_1, out half3 Color_2, out half DistanceAtten_3, out half ShadowAtten_4)
        {
            half3 _CustomFunction_2BF1432_Direction_0;
            half3 _CustomFunction_2BF1432_Color_1;
            half _CustomFunction_2BF1432_DistanceAtten_3;
            half _CustomFunction_2BF1432_ShadowAtten_4;
            MainLight_half(IN.AbsoluteWorldSpacePosition, _CustomFunction_2BF1432_Direction_0, _CustomFunction_2BF1432_Color_1, _CustomFunction_2BF1432_DistanceAtten_3, _CustomFunction_2BF1432_ShadowAtten_4);
            Direction_1 = _CustomFunction_2BF1432_Direction_0;
            Color_2 = _CustomFunction_2BF1432_Color_1;
            DistanceAtten_3 = _CustomFunction_2BF1432_DistanceAtten_3;
            ShadowAtten_4 = _CustomFunction_2BF1432_ShadowAtten_4;
        }

        void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
        {
            Out = dot(A, B);
        }

        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }

        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Preview_float3(float3 In, out float3 Out)
        {
            Out = In;
        }

        struct Bindings_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
        };

        void SG_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3(half4 Color_983329E6, half Vector1_B5019124, half3 Vector3_CC4B8202, half4 Color_7E2AEDE7, Bindings_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3 IN, out half3 Out_0)
        {
            half4 _Property_B9BD0120_Out_0 = Color_983329E6;
            half _Property_A1A748A2_Out_0 = Vector1_B5019124;
            half3 _Property_3BA98C39_Out_0 = Vector3_CC4B8202;
            half4 _Property_37A7735C_Out_0 = Color_7E2AEDE7;
            half3 _CustomFunction_866682A1_Out_6;
            DirectSpecular_half((_Property_B9BD0120_Out_0.xyz), _Property_A1A748A2_Out_0, _Property_3BA98C39_Out_0, (_Property_37A7735C_Out_0.xyz), IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _CustomFunction_866682A1_Out_6);
            Out_0 = _CustomFunction_866682A1_Out_6;
        }

        struct Bindings_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 AbsoluteWorldSpacePosition;
        };

        void SG_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9(float4 Color_43AD6CD2, float Vector1_356A410D, Bindings_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9 IN, out float3 Diffuse_0, out float3 Specular_1)
        {
            Bindings_GetMainLight_52012c17518825a429793d26daee4a8c _GetMainLight_862B78CC;
            _GetMainLight_862B78CC.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            half3 _GetMainLight_862B78CC_Direction_1;
            half3 _GetMainLight_862B78CC_Color_2;
            half _GetMainLight_862B78CC_DistanceAtten_3;
            half _GetMainLight_862B78CC_ShadowAtten_4;
            SG_GetMainLight_52012c17518825a429793d26daee4a8c(_GetMainLight_862B78CC, _GetMainLight_862B78CC_Direction_1, _GetMainLight_862B78CC_Color_2, _GetMainLight_862B78CC_DistanceAtten_3, _GetMainLight_862B78CC_ShadowAtten_4);
            float _DotProduct_35EBB205_Out_2;
            Unity_DotProduct_float3(IN.WorldSpaceNormal, _GetMainLight_862B78CC_Direction_1, _DotProduct_35EBB205_Out_2);
            float _Saturate_5DE62EF8_Out_1;
            Unity_Saturate_float(_DotProduct_35EBB205_Out_2, _Saturate_5DE62EF8_Out_1);
            float _Multiply_23A87056_Out_2;
            Unity_Multiply_float(_GetMainLight_862B78CC_DistanceAtten_3, _GetMainLight_862B78CC_ShadowAtten_4, _Multiply_23A87056_Out_2);
            float3 _Multiply_AFF983B9_Out_2;
            Unity_Multiply_float(_GetMainLight_862B78CC_Color_2, (_Multiply_23A87056_Out_2.xxx), _Multiply_AFF983B9_Out_2);
            float3 _Multiply_F761076F_Out_2;
            Unity_Multiply_float((_Saturate_5DE62EF8_Out_1.xxx), _Multiply_AFF983B9_Out_2, _Multiply_F761076F_Out_2);
            float4 _Property_75D6594C_Out_0 = Color_43AD6CD2;
            float _Property_55DA4F97_Out_0 = Vector1_356A410D;
            float3 _Preview_28D54188_Out_1;
            Unity_Preview_float3(_GetMainLight_862B78CC_Direction_1, _Preview_28D54188_Out_1);
            Bindings_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3 _DirectSpecular_EC3BC4FF;
            _DirectSpecular_EC3BC4FF.WorldSpaceNormal = IN.WorldSpaceNormal;
            _DirectSpecular_EC3BC4FF.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            half3 _DirectSpecular_EC3BC4FF_Out_0;
            SG_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3(_Property_75D6594C_Out_0, _Property_55DA4F97_Out_0, _Preview_28D54188_Out_1, (half4(_Multiply_AFF983B9_Out_2, 1.0)), _DirectSpecular_EC3BC4FF, _DirectSpecular_EC3BC4FF_Out_0);
            Diffuse_0 = _Multiply_F761076F_Out_2;
            Specular_1 = _DirectSpecular_EC3BC4FF_Out_0;
        }

        struct Bindings_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 AbsoluteWorldSpacePosition;
        };

        void SG_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b(half4 Color_EE85B5ED, half Vector1_B1513F4D, Bindings_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b IN, out half3 Diffuse_0, out half3 Specular_1)
        {
            half4 _Property_E47F52F0_Out_0 = Color_EE85B5ED;
            half _Property_16CA5CB6_Out_0 = Vector1_B1513F4D;
            half3 _CustomFunction_D3E20B4_Diffuse_5;
            half3 _CustomFunction_D3E20B4_Specular_6;
            AdditionalLights_half((_Property_E47F52F0_Out_0.xyz), _Property_16CA5CB6_Out_0, (IN.AbsoluteWorldSpacePosition).x, IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _CustomFunction_D3E20B4_Diffuse_5, _CustomFunction_D3E20B4_Specular_6);
            Diffuse_0 = _CustomFunction_D3E20B4_Diffuse_5;
            Specular_1 = _CustomFunction_D3E20B4_Specular_6;
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_CalculateLighting_22889f65f15d7fd41bcede954958ebe0
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 AbsoluteWorldSpacePosition;
        };

        void SG_CalculateLighting_22889f65f15d7fd41bcede954958ebe0(float4 Color_F3D622DA, float Vector1_D271B9C0, Bindings_CalculateLighting_22889f65f15d7fd41bcede954958ebe0 IN, out float3 Diffuse_1, out float3 Specular_2)
        {
            float4 _Property_FBB6E417_Out_0 = Color_F3D622DA;
            float _Property_1AC5122E_Out_0 = Vector1_D271B9C0;
            Bindings_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9 _CalculateMainLight_8017F8FC;
            _CalculateMainLight_8017F8FC.WorldSpaceNormal = IN.WorldSpaceNormal;
            _CalculateMainLight_8017F8FC.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            _CalculateMainLight_8017F8FC.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            float3 _CalculateMainLight_8017F8FC_Diffuse_0;
            float3 _CalculateMainLight_8017F8FC_Specular_1;
            SG_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9(_Property_FBB6E417_Out_0, _Property_1AC5122E_Out_0, _CalculateMainLight_8017F8FC, _CalculateMainLight_8017F8FC_Diffuse_0, _CalculateMainLight_8017F8FC_Specular_1);
            Bindings_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b _CalculateAdditionalLights_45C89447;
            _CalculateAdditionalLights_45C89447.WorldSpaceNormal = IN.WorldSpaceNormal;
            _CalculateAdditionalLights_45C89447.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            _CalculateAdditionalLights_45C89447.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            half3 _CalculateAdditionalLights_45C89447_Diffuse_0;
            half3 _CalculateAdditionalLights_45C89447_Specular_1;
            SG_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b(_Property_FBB6E417_Out_0, _Property_1AC5122E_Out_0, _CalculateAdditionalLights_45C89447, _CalculateAdditionalLights_45C89447_Diffuse_0, _CalculateAdditionalLights_45C89447_Specular_1);
            float3 _Add_455A5023_Out_2;
            Unity_Add_float3(_CalculateMainLight_8017F8FC_Diffuse_0, _CalculateAdditionalLights_45C89447_Diffuse_0, _Add_455A5023_Out_2);
            float3 _Add_97936D8C_Out_2;
            Unity_Add_float3(_CalculateMainLight_8017F8FC_Specular_1, _CalculateAdditionalLights_45C89447_Specular_1, _Add_97936D8C_Out_2);
            Diffuse_1 = _Add_455A5023_Out_2;
            Specular_2 = _Add_97936D8C_Out_2;
        }

        void Unity_ColorspaceConversion_RGB_HSV_float(float3 In, out float3 Out)
        {
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float  E = 1e-10;
            Out = float3(abs(Q.z + (Q.w - Q.y) / (6.0 * D + E)), D / (Q.x + E), Q.x);
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        // Graph Vertex
        // GraphVertex: <None>

        // Graph Pixel
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 TangentSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 AbsoluteWorldSpacePosition;
            float4 uv0;
        };

        struct SurfaceDescription
        {
            float3 Albedo;
            float3 Normal;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _SampleTexture2D_C5EF5AEE_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
            float _SampleTexture2D_C5EF5AEE_R_4 = _SampleTexture2D_C5EF5AEE_RGBA_0.r;
            float _SampleTexture2D_C5EF5AEE_G_5 = _SampleTexture2D_C5EF5AEE_RGBA_0.g;
            float _SampleTexture2D_C5EF5AEE_B_6 = _SampleTexture2D_C5EF5AEE_RGBA_0.b;
            float _SampleTexture2D_C5EF5AEE_A_7 = _SampleTexture2D_C5EF5AEE_RGBA_0.a;
            float4 _Property_B15FCF53_Out_0 = _Specular;
            float _Property_ABA16D75_Out_0 = _Smoothness;
            Bindings_CalculateLighting_22889f65f15d7fd41bcede954958ebe0 _CalculateLighting_C9922456;
            _CalculateLighting_C9922456.WorldSpaceNormal = IN.WorldSpaceNormal;
            _CalculateLighting_C9922456.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            _CalculateLighting_C9922456.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            float3 _CalculateLighting_C9922456_Diffuse_1;
            float3 _CalculateLighting_C9922456_Specular_2;
            SG_CalculateLighting_22889f65f15d7fd41bcede954958ebe0(_Property_B15FCF53_Out_0, _Property_ABA16D75_Out_0, _CalculateLighting_C9922456, _CalculateLighting_C9922456_Diffuse_1, _CalculateLighting_C9922456_Specular_2);
            float3 _ColorspaceConversion_4BA53D8_Out_1;
            Unity_ColorspaceConversion_RGB_HSV_float(_CalculateLighting_C9922456_Diffuse_1, _ColorspaceConversion_4BA53D8_Out_1);
            float _Split_311429B7_R_1 = _ColorspaceConversion_4BA53D8_Out_1[0];
            float _Split_311429B7_G_2 = _ColorspaceConversion_4BA53D8_Out_1[1];
            float _Split_311429B7_B_3 = _ColorspaceConversion_4BA53D8_Out_1[2];
            float _Split_311429B7_A_4 = 0;
            float2 _Vector2_1E36EEA3_Out_0 = float2(_Split_311429B7_B_3, 0);
            SamplerState _Property_B762A553_Out_0 = SamplerState_Point_Clamp;
            float4 _SampleTexture2DLOD_7628179_RGBA_0 = SAMPLE_TEXTURE2D_LOD(_LightingRamp, _Property_B762A553_Out_0, _Vector2_1E36EEA3_Out_0, 0);
            float _SampleTexture2DLOD_7628179_R_5 = _SampleTexture2DLOD_7628179_RGBA_0.r;
            float _SampleTexture2DLOD_7628179_G_6 = _SampleTexture2DLOD_7628179_RGBA_0.g;
            float _SampleTexture2DLOD_7628179_B_7 = _SampleTexture2DLOD_7628179_RGBA_0.b;
            float _SampleTexture2DLOD_7628179_A_8 = _SampleTexture2DLOD_7628179_RGBA_0.a;
            float4 _Property_650A600B_Out_0 = _Color;
            float4 _Multiply_ECEFA98C_Out_2;
            Unity_Multiply_float(_SampleTexture2DLOD_7628179_RGBA_0, _Property_650A600B_Out_0, _Multiply_ECEFA98C_Out_2);
            float3 _ColorspaceConversion_A59FDF58_Out_1;
            Unity_ColorspaceConversion_RGB_HSV_float(_CalculateLighting_C9922456_Specular_2, _ColorspaceConversion_A59FDF58_Out_1);
            float _Split_7FF92099_R_1 = _ColorspaceConversion_A59FDF58_Out_1[0];
            float _Split_7FF92099_G_2 = _ColorspaceConversion_A59FDF58_Out_1[1];
            float _Split_7FF92099_B_3 = _ColorspaceConversion_A59FDF58_Out_1[2];
            float _Split_7FF92099_A_4 = 0;
            float2 _Vector2_BC980998_Out_0 = float2(_Split_7FF92099_B_3, 1);
            SamplerState _Property_DD9A80FA_Out_0 = SamplerState_Point_Clamp;
            float4 _SampleTexture2DLOD_4DCDC610_RGBA_0 = SAMPLE_TEXTURE2D_LOD(_LightingRamp, _Property_DD9A80FA_Out_0, _Vector2_BC980998_Out_0, 0);
            float _SampleTexture2DLOD_4DCDC610_R_5 = _SampleTexture2DLOD_4DCDC610_RGBA_0.r;
            float _SampleTexture2DLOD_4DCDC610_G_6 = _SampleTexture2DLOD_4DCDC610_RGBA_0.g;
            float _SampleTexture2DLOD_4DCDC610_B_7 = _SampleTexture2DLOD_4DCDC610_RGBA_0.b;
            float _SampleTexture2DLOD_4DCDC610_A_8 = _SampleTexture2DLOD_4DCDC610_RGBA_0.a;
            float4 _Add_25A2598_Out_2;
            Unity_Add_float4(_Multiply_ECEFA98C_Out_2, _SampleTexture2DLOD_4DCDC610_RGBA_0, _Add_25A2598_Out_2);
            float4 _Multiply_7EC037BE_Out_2;
            Unity_Multiply_float(_SampleTexture2D_C5EF5AEE_RGBA_0, _Add_25A2598_Out_2, _Multiply_7EC037BE_Out_2);
            float4 _Property_AAB2121C_Out_0 = _Color;
            float _Split_7F12A9B1_R_1 = _Property_AAB2121C_Out_0[0];
            float _Split_7F12A9B1_G_2 = _Property_AAB2121C_Out_0[1];
            float _Split_7F12A9B1_B_3 = _Property_AAB2121C_Out_0[2];
            float _Split_7F12A9B1_A_4 = _Property_AAB2121C_Out_0[3];
            surface.Albedo = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Normal = IN.TangentSpaceNormal;
            surface.Emission = lerp(_Multiply_7EC037BE_Out_2.xyz, dot(_Multiply_7EC037BE_Out_2.xyz, GrayScaleColor), GrayScaleFactor);
            surface.Metallic = 0;
            surface.Smoothness = 0.5;
            surface.Occlusion = 1;
            surface.Alpha = _Split_7F12A9B1_A_4;
            surface.AlphaClipThreshold = 0.0;
            return surface;
        }

        // --------------------------------------------------
        // Structs and Packing

        // Generated Type: Attributes
        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };

        // Generated Type: Varyings
        struct Varyings
        {
            float4 positionCS : SV_Position;
            float3 positionWS;
            float3 normalWS;
            float4 tangentWS;
            float4 texCoord0;
            float3 viewDirectionWS;
            float3 bitangentWS;
            #if defined(LIGHTMAP_ON)
            float2 lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            float3 sh;
            #endif
            float4 fogFactorAndVertexLight;
            float4 shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
        };

        // Generated Type: PackedVaryings
        struct PackedVaryings
        {
            float4 positionCS : SV_Position;
            #if defined(LIGHTMAP_ON)
            #endif
            #if !defined(LIGHTMAP_ON)
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            float3 interp00 : TEXCOORD0;
            float3 interp01 : TEXCOORD1;
            float4 interp02 : TEXCOORD2;
            float4 interp03 : TEXCOORD3;
            float3 interp04 : TEXCOORD4;
            float3 interp05 : TEXCOORD5;
            float2 interp06 : TEXCOORD6;
            float3 interp07 : TEXCOORD7;
            float4 interp08 : TEXCOORD8;
            float4 interp09 : TEXCOORD9;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        // Packed Type: Varyings
        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp00.xyz = input.positionWS;
            output.interp01.xyz = input.normalWS;
            output.interp02.xyzw = input.tangentWS;
            output.interp03.xyzw = input.texCoord0;
            output.interp04.xyz = input.viewDirectionWS;
            output.interp05.xyz = input.bitangentWS;
            #if defined(LIGHTMAP_ON)
            output.interp06.xy = input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp07.xyz = input.sh;
            #endif
            output.interp08.xyzw = input.fogFactorAndVertexLight;
            output.interp09.xyzw = input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            return output;
        }

        // Unpacked Type: Varyings
        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp00.xyz;
            output.normalWS = input.interp01.xyz;
            output.tangentWS = input.interp02.xyzw;
            output.texCoord0 = input.interp03.xyzw;
            output.viewDirectionWS = input.interp04.xyz;
            output.bitangentWS = input.interp05.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp06.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp07.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp08.xyzw;
            output.shadowCoord = input.interp09.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            return output;
        }

        // --------------------------------------------------
        // Build Graph Inputs

        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

            output.WorldSpaceNormal = input.normalWS;
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
            output.WorldSpaceViewDirection = input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

            return output;
        }


        // --------------------------------------------------
        // Main

        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

        ENDHLSL
    }

    Pass
    {
        Name "ShadowCaster"
        Tags
        {
            "LightMode" = "ShadowCaster"
        }

            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_SHADOWCASTER

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _Specular;
            float _Smoothness;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            TEXTURE2D(_LightingRamp); SAMPLER(sampler_LightingRamp); float4 _LightingRamp_TexelSize;
            SAMPLER(SamplerState_Point_Clamp);

            // Graph Functions
            // GraphFunctions: <None>

            // Graph Vertex
            // GraphVertex: <None>

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
            };

            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_AAB2121C_Out_0 = _Color;
                float _Split_7F12A9B1_R_1 = _Property_AAB2121C_Out_0[0];
                float _Split_7F12A9B1_G_2 = _Property_AAB2121C_Out_0[1];
                float _Split_7F12A9B1_B_3 = _Property_AAB2121C_Out_0[2];
                float _Split_7F12A9B1_A_4 = _Property_AAB2121C_Out_0[3];
                surface.Alpha = _Split_7F12A9B1_A_4;
                surface.AlphaClipThreshold = 0.0;
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };

            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

                // Render State
                Blend One Zero, One Zero
                Cull Back
                ZTest LEqual
                ZWrite On
                ColorMask 0


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0
                #pragma multi_compile_instancing

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines
                #define _AlphaClip 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define SHADERPASS_DEPTHONLY

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _Specular;
                float _Smoothness;
                CBUFFER_END
                TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                TEXTURE2D(_LightingRamp); SAMPLER(sampler_LightingRamp); float4 _LightingRamp_TexelSize;
                SAMPLER(SamplerState_Point_Clamp);

                // Graph Functions
                // GraphFunctions: <None>

                // Graph Vertex
                // GraphVertex: <None>

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                };

                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _Property_AAB2121C_Out_0 = _Color;
                    float _Split_7F12A9B1_R_1 = _Property_AAB2121C_Out_0[0];
                    float _Split_7F12A9B1_G_2 = _Property_AAB2121C_Out_0[1];
                    float _Split_7F12A9B1_B_3 = _Property_AAB2121C_Out_0[2];
                    float _Split_7F12A9B1_A_4 = _Property_AAB2121C_Out_0[3];
                    surface.Alpha = _Split_7F12A9B1_A_4;
                    surface.AlphaClipThreshold = 0.0;
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    float4 positionCS : SV_Position;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                };

                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_Position;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                ENDHLSL
            }

            Pass
            {
                Name "Meta"
                Tags
                {
                    "LightMode" = "Meta"
                }

                    // Render State
                    Blend One Zero, One Zero
                    Cull Back
                    ZTest LEqual
                    ZWrite On
                    // ColorMask: <None>


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0

                    // Keywords
                    #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                    // GraphKeywords: <None>

                    // Defines
                    #define _AlphaClip 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define ATTRIBUTES_NEED_TEXCOORD1
                    #define ATTRIBUTES_NEED_TEXCOORD2
                    #define VARYINGS_NEED_POSITION_WS 
                    #define VARYINGS_NEED_NORMAL_WS
                    #define VARYINGS_NEED_TEXCOORD0
                    #define VARYINGS_NEED_VIEWDIRECTION_WS
                    #define SHADERPASS_META

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float4 _Color;
                    float4 _Specular;
                    float _Smoothness;
                    CBUFFER_END
                    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                    TEXTURE2D(_LightingRamp); SAMPLER(sampler_LightingRamp); float4 _LightingRamp_TexelSize;
                    SAMPLER(SamplerState_Point_Clamp);
                    SAMPLER(_SampleTexture2D_C5EF5AEE_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    // 10567835598322a437e189e9044dec2b
                    #include "Assets/Project/Scripts/Shaders/Includes/CustomLighting.hlsl"

                    struct Bindings_GetMainLight_52012c17518825a429793d26daee4a8c
                    {
                        float3 AbsoluteWorldSpacePosition;
                    };

                    void SG_GetMainLight_52012c17518825a429793d26daee4a8c(Bindings_GetMainLight_52012c17518825a429793d26daee4a8c IN, out half3 Direction_1, out half3 Color_2, out half DistanceAtten_3, out half ShadowAtten_4)
                    {
                        half3 _CustomFunction_2BF1432_Direction_0;
                        half3 _CustomFunction_2BF1432_Color_1;
                        half _CustomFunction_2BF1432_DistanceAtten_3;
                        half _CustomFunction_2BF1432_ShadowAtten_4;
                        MainLight_half(IN.AbsoluteWorldSpacePosition, _CustomFunction_2BF1432_Direction_0, _CustomFunction_2BF1432_Color_1, _CustomFunction_2BF1432_DistanceAtten_3, _CustomFunction_2BF1432_ShadowAtten_4);
                        Direction_1 = _CustomFunction_2BF1432_Direction_0;
                        Color_2 = _CustomFunction_2BF1432_Color_1;
                        DistanceAtten_3 = _CustomFunction_2BF1432_DistanceAtten_3;
                        ShadowAtten_4 = _CustomFunction_2BF1432_ShadowAtten_4;
                    }

                    void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
                    {
                        Out = dot(A, B);
                    }

                    void Unity_Saturate_float(float In, out float Out)
                    {
                        Out = saturate(In);
                    }

                    void Unity_Multiply_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Preview_float3(float3 In, out float3 Out)
                    {
                        Out = In;
                    }

                    struct Bindings_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceViewDirection;
                    };

                    void SG_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3(half4 Color_983329E6, half Vector1_B5019124, half3 Vector3_CC4B8202, half4 Color_7E2AEDE7, Bindings_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3 IN, out half3 Out_0)
                    {
                        half4 _Property_B9BD0120_Out_0 = Color_983329E6;
                        half _Property_A1A748A2_Out_0 = Vector1_B5019124;
                        half3 _Property_3BA98C39_Out_0 = Vector3_CC4B8202;
                        half4 _Property_37A7735C_Out_0 = Color_7E2AEDE7;
                        half3 _CustomFunction_866682A1_Out_6;
                        DirectSpecular_half((_Property_B9BD0120_Out_0.xyz), _Property_A1A748A2_Out_0, _Property_3BA98C39_Out_0, (_Property_37A7735C_Out_0.xyz), IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _CustomFunction_866682A1_Out_6);
                        Out_0 = _CustomFunction_866682A1_Out_6;
                    }

                    struct Bindings_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceViewDirection;
                        float3 AbsoluteWorldSpacePosition;
                    };

                    void SG_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9(float4 Color_43AD6CD2, float Vector1_356A410D, Bindings_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9 IN, out float3 Diffuse_0, out float3 Specular_1)
                    {
                        Bindings_GetMainLight_52012c17518825a429793d26daee4a8c _GetMainLight_862B78CC;
                        _GetMainLight_862B78CC.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                        half3 _GetMainLight_862B78CC_Direction_1;
                        half3 _GetMainLight_862B78CC_Color_2;
                        half _GetMainLight_862B78CC_DistanceAtten_3;
                        half _GetMainLight_862B78CC_ShadowAtten_4;
                        SG_GetMainLight_52012c17518825a429793d26daee4a8c(_GetMainLight_862B78CC, _GetMainLight_862B78CC_Direction_1, _GetMainLight_862B78CC_Color_2, _GetMainLight_862B78CC_DistanceAtten_3, _GetMainLight_862B78CC_ShadowAtten_4);
                        float _DotProduct_35EBB205_Out_2;
                        Unity_DotProduct_float3(IN.WorldSpaceNormal, _GetMainLight_862B78CC_Direction_1, _DotProduct_35EBB205_Out_2);
                        float _Saturate_5DE62EF8_Out_1;
                        Unity_Saturate_float(_DotProduct_35EBB205_Out_2, _Saturate_5DE62EF8_Out_1);
                        float _Multiply_23A87056_Out_2;
                        Unity_Multiply_float(_GetMainLight_862B78CC_DistanceAtten_3, _GetMainLight_862B78CC_ShadowAtten_4, _Multiply_23A87056_Out_2);
                        float3 _Multiply_AFF983B9_Out_2;
                        Unity_Multiply_float(_GetMainLight_862B78CC_Color_2, (_Multiply_23A87056_Out_2.xxx), _Multiply_AFF983B9_Out_2);
                        float3 _Multiply_F761076F_Out_2;
                        Unity_Multiply_float((_Saturate_5DE62EF8_Out_1.xxx), _Multiply_AFF983B9_Out_2, _Multiply_F761076F_Out_2);
                        float4 _Property_75D6594C_Out_0 = Color_43AD6CD2;
                        float _Property_55DA4F97_Out_0 = Vector1_356A410D;
                        float3 _Preview_28D54188_Out_1;
                        Unity_Preview_float3(_GetMainLight_862B78CC_Direction_1, _Preview_28D54188_Out_1);
                        Bindings_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3 _DirectSpecular_EC3BC4FF;
                        _DirectSpecular_EC3BC4FF.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _DirectSpecular_EC3BC4FF.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
                        half3 _DirectSpecular_EC3BC4FF_Out_0;
                        SG_DirectSpecular_a9cf43e29a492634eb6636b1384dd6c3(_Property_75D6594C_Out_0, _Property_55DA4F97_Out_0, _Preview_28D54188_Out_1, (half4(_Multiply_AFF983B9_Out_2, 1.0)), _DirectSpecular_EC3BC4FF, _DirectSpecular_EC3BC4FF_Out_0);
                        Diffuse_0 = _Multiply_F761076F_Out_2;
                        Specular_1 = _DirectSpecular_EC3BC4FF_Out_0;
                    }

                    struct Bindings_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceViewDirection;
                        float3 AbsoluteWorldSpacePosition;
                    };

                    void SG_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b(half4 Color_EE85B5ED, half Vector1_B1513F4D, Bindings_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b IN, out half3 Diffuse_0, out half3 Specular_1)
                    {
                        half4 _Property_E47F52F0_Out_0 = Color_EE85B5ED;
                        half _Property_16CA5CB6_Out_0 = Vector1_B1513F4D;
                        half3 _CustomFunction_D3E20B4_Diffuse_5;
                        half3 _CustomFunction_D3E20B4_Specular_6;
                        AdditionalLights_half((_Property_E47F52F0_Out_0.xyz), _Property_16CA5CB6_Out_0, (IN.AbsoluteWorldSpacePosition).x, IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _CustomFunction_D3E20B4_Diffuse_5, _CustomFunction_D3E20B4_Specular_6);
                        Diffuse_0 = _CustomFunction_D3E20B4_Diffuse_5;
                        Specular_1 = _CustomFunction_D3E20B4_Specular_6;
                    }

                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }

                    struct Bindings_CalculateLighting_22889f65f15d7fd41bcede954958ebe0
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceViewDirection;
                        float3 AbsoluteWorldSpacePosition;
                    };

                    void SG_CalculateLighting_22889f65f15d7fd41bcede954958ebe0(float4 Color_F3D622DA, float Vector1_D271B9C0, Bindings_CalculateLighting_22889f65f15d7fd41bcede954958ebe0 IN, out float3 Diffuse_1, out float3 Specular_2)
                    {
                        float4 _Property_FBB6E417_Out_0 = Color_F3D622DA;
                        float _Property_1AC5122E_Out_0 = Vector1_D271B9C0;
                        Bindings_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9 _CalculateMainLight_8017F8FC;
                        _CalculateMainLight_8017F8FC.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _CalculateMainLight_8017F8FC.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
                        _CalculateMainLight_8017F8FC.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                        float3 _CalculateMainLight_8017F8FC_Diffuse_0;
                        float3 _CalculateMainLight_8017F8FC_Specular_1;
                        SG_CalculateMainLight_39b1287c70bb7dd4e87fae9a576f53d9(_Property_FBB6E417_Out_0, _Property_1AC5122E_Out_0, _CalculateMainLight_8017F8FC, _CalculateMainLight_8017F8FC_Diffuse_0, _CalculateMainLight_8017F8FC_Specular_1);
                        Bindings_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b _CalculateAdditionalLights_45C89447;
                        _CalculateAdditionalLights_45C89447.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _CalculateAdditionalLights_45C89447.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
                        _CalculateAdditionalLights_45C89447.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                        half3 _CalculateAdditionalLights_45C89447_Diffuse_0;
                        half3 _CalculateAdditionalLights_45C89447_Specular_1;
                        SG_CalculateAdditionalLights_8578e370a0c1d0145a8a7724eaf3658b(_Property_FBB6E417_Out_0, _Property_1AC5122E_Out_0, _CalculateAdditionalLights_45C89447, _CalculateAdditionalLights_45C89447_Diffuse_0, _CalculateAdditionalLights_45C89447_Specular_1);
                        float3 _Add_455A5023_Out_2;
                        Unity_Add_float3(_CalculateMainLight_8017F8FC_Diffuse_0, _CalculateAdditionalLights_45C89447_Diffuse_0, _Add_455A5023_Out_2);
                        float3 _Add_97936D8C_Out_2;
                        Unity_Add_float3(_CalculateMainLight_8017F8FC_Specular_1, _CalculateAdditionalLights_45C89447_Specular_1, _Add_97936D8C_Out_2);
                        Diffuse_1 = _Add_455A5023_Out_2;
                        Specular_2 = _Add_97936D8C_Out_2;
                    }

                    void Unity_ColorspaceConversion_RGB_HSV_float(float3 In, out float3 Out)
                    {
                        float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                        float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                        float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                        float D = Q.x - min(Q.w, Q.y);
                        float  E = 1e-10;
                        Out = float3(abs(Q.z + (Q.w - Q.y) / (6.0 * D + E)), D / (Q.x + E), Q.x);
                    }

                    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                    {
                        Out = A + B;
                    }

                    // Graph Vertex
                    // GraphVertex: <None>

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                        float3 WorldSpaceNormal;
                        float3 TangentSpaceNormal;
                        float3 WorldSpaceViewDirection;
                        float3 AbsoluteWorldSpacePosition;
                        float4 uv0;
                    };

                    struct SurfaceDescription
                    {
                        float3 Albedo;
                        float3 Emission;
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_C5EF5AEE_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                        float _SampleTexture2D_C5EF5AEE_R_4 = _SampleTexture2D_C5EF5AEE_RGBA_0.r;
                        float _SampleTexture2D_C5EF5AEE_G_5 = _SampleTexture2D_C5EF5AEE_RGBA_0.g;
                        float _SampleTexture2D_C5EF5AEE_B_6 = _SampleTexture2D_C5EF5AEE_RGBA_0.b;
                        float _SampleTexture2D_C5EF5AEE_A_7 = _SampleTexture2D_C5EF5AEE_RGBA_0.a;
                        float4 _Property_B15FCF53_Out_0 = _Specular;
                        float _Property_ABA16D75_Out_0 = _Smoothness;
                        Bindings_CalculateLighting_22889f65f15d7fd41bcede954958ebe0 _CalculateLighting_C9922456;
                        _CalculateLighting_C9922456.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _CalculateLighting_C9922456.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
                        _CalculateLighting_C9922456.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
                        float3 _CalculateLighting_C9922456_Diffuse_1;
                        float3 _CalculateLighting_C9922456_Specular_2;
                        SG_CalculateLighting_22889f65f15d7fd41bcede954958ebe0(_Property_B15FCF53_Out_0, _Property_ABA16D75_Out_0, _CalculateLighting_C9922456, _CalculateLighting_C9922456_Diffuse_1, _CalculateLighting_C9922456_Specular_2);
                        float3 _ColorspaceConversion_4BA53D8_Out_1;
                        Unity_ColorspaceConversion_RGB_HSV_float(_CalculateLighting_C9922456_Diffuse_1, _ColorspaceConversion_4BA53D8_Out_1);
                        float _Split_311429B7_R_1 = _ColorspaceConversion_4BA53D8_Out_1[0];
                        float _Split_311429B7_G_2 = _ColorspaceConversion_4BA53D8_Out_1[1];
                        float _Split_311429B7_B_3 = _ColorspaceConversion_4BA53D8_Out_1[2];
                        float _Split_311429B7_A_4 = 0;
                        float2 _Vector2_1E36EEA3_Out_0 = float2(_Split_311429B7_B_3, 0);
                        SamplerState _Property_B762A553_Out_0 = SamplerState_Point_Clamp;
                        float4 _SampleTexture2DLOD_7628179_RGBA_0 = SAMPLE_TEXTURE2D_LOD(_LightingRamp, _Property_B762A553_Out_0, _Vector2_1E36EEA3_Out_0, 0);
                        float _SampleTexture2DLOD_7628179_R_5 = _SampleTexture2DLOD_7628179_RGBA_0.r;
                        float _SampleTexture2DLOD_7628179_G_6 = _SampleTexture2DLOD_7628179_RGBA_0.g;
                        float _SampleTexture2DLOD_7628179_B_7 = _SampleTexture2DLOD_7628179_RGBA_0.b;
                        float _SampleTexture2DLOD_7628179_A_8 = _SampleTexture2DLOD_7628179_RGBA_0.a;
                        float4 _Property_650A600B_Out_0 = _Color;
                        float4 _Multiply_ECEFA98C_Out_2;
                        Unity_Multiply_float(_SampleTexture2DLOD_7628179_RGBA_0, _Property_650A600B_Out_0, _Multiply_ECEFA98C_Out_2);
                        float3 _ColorspaceConversion_A59FDF58_Out_1;
                        Unity_ColorspaceConversion_RGB_HSV_float(_CalculateLighting_C9922456_Specular_2, _ColorspaceConversion_A59FDF58_Out_1);
                        float _Split_7FF92099_R_1 = _ColorspaceConversion_A59FDF58_Out_1[0];
                        float _Split_7FF92099_G_2 = _ColorspaceConversion_A59FDF58_Out_1[1];
                        float _Split_7FF92099_B_3 = _ColorspaceConversion_A59FDF58_Out_1[2];
                        float _Split_7FF92099_A_4 = 0;
                        float2 _Vector2_BC980998_Out_0 = float2(_Split_7FF92099_B_3, 1);
                        SamplerState _Property_DD9A80FA_Out_0 = SamplerState_Point_Clamp;
                        float4 _SampleTexture2DLOD_4DCDC610_RGBA_0 = SAMPLE_TEXTURE2D_LOD(_LightingRamp, _Property_DD9A80FA_Out_0, _Vector2_BC980998_Out_0, 0);
                        float _SampleTexture2DLOD_4DCDC610_R_5 = _SampleTexture2DLOD_4DCDC610_RGBA_0.r;
                        float _SampleTexture2DLOD_4DCDC610_G_6 = _SampleTexture2DLOD_4DCDC610_RGBA_0.g;
                        float _SampleTexture2DLOD_4DCDC610_B_7 = _SampleTexture2DLOD_4DCDC610_RGBA_0.b;
                        float _SampleTexture2DLOD_4DCDC610_A_8 = _SampleTexture2DLOD_4DCDC610_RGBA_0.a;
                        float4 _Add_25A2598_Out_2;
                        Unity_Add_float4(_Multiply_ECEFA98C_Out_2, _SampleTexture2DLOD_4DCDC610_RGBA_0, _Add_25A2598_Out_2);
                        float4 _Multiply_7EC037BE_Out_2;
                        Unity_Multiply_float(_SampleTexture2D_C5EF5AEE_RGBA_0, _Add_25A2598_Out_2, _Multiply_7EC037BE_Out_2);
                        float4 _Property_AAB2121C_Out_0 = _Color;
                        float _Split_7F12A9B1_R_1 = _Property_AAB2121C_Out_0[0];
                        float _Split_7F12A9B1_G_2 = _Property_AAB2121C_Out_0[1];
                        float _Split_7F12A9B1_B_3 = _Property_AAB2121C_Out_0[2];
                        float _Split_7F12A9B1_A_4 = _Property_AAB2121C_Out_0[3];
                        surface.Albedo = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                        surface.Emission = (_Multiply_7EC037BE_Out_2.xyz);
                        surface.Alpha = _Split_7F12A9B1_A_4;
                        surface.AlphaClipThreshold = 0.0;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        float4 uv0 : TEXCOORD0;
                        float4 uv1 : TEXCOORD1;
                        float4 uv2 : TEXCOORD2;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        float4 positionCS : SV_Position;
                        float3 positionWS;
                        float3 normalWS;
                        float4 texCoord0;
                        float3 viewDirectionWS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                    };

                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_Position;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        float3 interp00 : TEXCOORD0;
                        float3 interp01 : TEXCOORD1;
                        float4 interp02 : TEXCOORD2;
                        float3 interp03 : TEXCOORD3;
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output;
                        output.positionCS = input.positionCS;
                        output.interp00.xyz = input.positionWS;
                        output.interp01.xyz = input.normalWS;
                        output.interp02.xyzw = input.texCoord0;
                        output.interp03.xyz = input.viewDirectionWS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output;
                        output.positionCS = input.positionCS;
                        output.positionWS = input.interp00.xyz;
                        output.normalWS = input.interp01.xyz;
                        output.texCoord0 = input.interp02.xyzw;
                        output.viewDirectionWS = input.interp03.xyz;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                        output.WorldSpaceNormal = input.normalWS;
                        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
                        output.WorldSpaceViewDirection = input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
                        output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
                        output.uv0 = input.texCoord0;
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                    ENDHLSL
                }

                Pass
                {
                        // Name: <None>
                        Tags
                        {
                            "LightMode" = "Universal2D"
                        }

                        // Render State
                        Blend One Zero, One Zero
                        Cull Back
                        ZTest LEqual
                        ZWrite On
                        // ColorMask: <None>


                        HLSLPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        // Pragmas
                        #pragma prefer_hlslcc gles
                        #pragma exclude_renderers d3d11_9x
                        #pragma target 2.0
                        #pragma multi_compile_instancing

                        // Keywords
                        // PassKeywords: <None>
                        // GraphKeywords: <None>

                        // Defines
                        #define _AlphaClip 1
                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define SHADERPASS_2D

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float4 _Color;
                        float4 _Specular;
                        float _Smoothness;
                        CBUFFER_END
                        TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                        TEXTURE2D(_LightingRamp); SAMPLER(sampler_LightingRamp); float4 _LightingRamp_TexelSize;
                        SAMPLER(SamplerState_Point_Clamp);

                        // Graph Functions
                        // GraphFunctions: <None>

                        // Graph Vertex
                        // GraphVertex: <None>

                        // Graph Pixel
                        struct SurfaceDescriptionInputs
                        {
                            float3 TangentSpaceNormal;
                        };

                        struct SurfaceDescription
                        {
                            float3 Albedo;
                            float Alpha;
                            float AlphaClipThreshold;
                        };

                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                        {
                            SurfaceDescription surface = (SurfaceDescription)0;
                            float4 _Property_AAB2121C_Out_0 = _Color;
                            float _Split_7F12A9B1_R_1 = _Property_AAB2121C_Out_0[0];
                            float _Split_7F12A9B1_G_2 = _Property_AAB2121C_Out_0[1];
                            float _Split_7F12A9B1_B_3 = _Property_AAB2121C_Out_0[2];
                            float _Split_7F12A9B1_A_4 = _Property_AAB2121C_Out_0[3];
                            surface.Albedo = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                            surface.Alpha = _Split_7F12A9B1_A_4;
                            surface.AlphaClipThreshold = 0.0;
                            return surface;
                        }

                        // --------------------------------------------------
                        // Structs and Packing

                        // Generated Type: Attributes
                        struct Attributes
                        {
                            float3 positionOS : POSITION;
                            float3 normalOS : NORMAL;
                            float4 tangentOS : TANGENT;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };

                        // Generated Type: Varyings
                        struct Varyings
                        {
                            float4 positionCS : SV_Position;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                        };

                        // Generated Type: PackedVaryings
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_Position;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Packed Type: Varyings
                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output;
                            output.positionCS = input.positionCS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            return output;
                        }

                        // Unpacked Type: Varyings
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output;
                            output.positionCS = input.positionCS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            return output;
                        }

                        // --------------------------------------------------
                        // Build Graph Inputs

                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                        {
                            SurfaceDescriptionInputs output;
                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                        #else
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                        #endif
                        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                        }


                        // --------------------------------------------------
                        // Main

                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                        ENDHLSL
                    }

    }
        FallBack "Hidden/Shader Graph/FallbackError"
}