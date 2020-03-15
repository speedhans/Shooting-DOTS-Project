using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;

[GenerateAuthoringComponent]
public struct LifeTimerComponent : IComponentData
{
    public float Value;

    public LifeTimerComponent(float _Value) { Value = _Value; }
}

public class LifeTimerSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    [Unity.Burst.BurstCompile]
    struct LifeTimerJob : IJobForEachWithEntity<LifeTimerComponent>
    {
        public float DeltaTime;
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public void Execute(Entity entity, int index, ref LifeTimerComponent c0)
        {
            c0.Value -= DeltaTime;
            if (c0.Value <= 0.0f)
            {
                CommandBuffer.DestroyEntity(index, entity);
            }
        }
    }

    protected override void OnCreate()
    {
        m_EndSimulationEcbSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new LifeTimerJob()
        {
            DeltaTime = Time.DeltaTime,
            CommandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);
        job.Complete();
        return job;
    }
}
