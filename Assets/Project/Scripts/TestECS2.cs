﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TestECS2 : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField]
    Material m_TrailMat;
    [SerializeField]
    int m_ParticleCount;
    public void Convert(Entity _entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        UnityEngine.Debug.Log("GameObjectConversionSystem");

        TrailRendererSystem.m_Material = m_TrailMat;

        EntityManager manager = dstManager;//World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype arch = manager.CreateArchetype(typeof(Translation), typeof(TrailVelocityComponent), typeof(TrailComponent), typeof(TrailBufferElement));
        Random random = new Random(99);
        for (int i = 0; i < m_ParticleCount; ++i)
        {
            Entity entity = manager.CreateEntity(arch);
            //manager.SetName(entity, "Trail_" + i.ToString());
            manager.SetComponentData(entity, new Translation() { Value = new float3(1, 1, 1) });
            manager.SetComponentData(entity, new TrailVelocityComponent() { Value = random.NextFloat3Direction() });
            manager.SetComponentData(entity, new TrailComponent() { MeshCount = 20 });
            DynamicBuffer<TrailBufferElement> buffer = manager.GetBuffer<TrailBufferElement>(entity);
            buffer.Add(new TrailBufferElement(new float3(1, 1, 1)));
        }
    }
}
