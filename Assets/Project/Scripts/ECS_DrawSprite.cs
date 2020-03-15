using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

public class ECS_DrawSprite : ComponentSystem
{
    protected override void OnUpdate()
    {
        //float s = math.sin((float)Time.ElapsedTime);

        Entities.ForEach((Entity entiti, ref TestComponent test, ref LocalToWorld localtoworld, ref NonUniformScale _Scale) =>
        {
            //_Scale.Value = new float3(1.0f + s, 1.0f + s, 1.0f);
            Graphics.DrawMesh(ECS_SpriteRenderer.m_sMesh, localtoworld.Value, ECS_SpriteRenderer.m_sMat, 0);
        });
    }
}
