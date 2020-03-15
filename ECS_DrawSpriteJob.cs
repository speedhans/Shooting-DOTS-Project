using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Transforms;

public class ECS_DrawSprite : JobComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entiti, ref TestComponent test, ref LocalToWorld localtoworld) =>
        {
            Graphics.DrawMesh(ECS_SpriteRenderer.m_sMesh, localtoworld.Value, ECS_SpriteRenderer.m_sMat, 0);
        });

    }
}
