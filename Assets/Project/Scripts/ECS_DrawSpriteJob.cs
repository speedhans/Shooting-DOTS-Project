using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

[DisableAutoCreation]
public class ECS_DrawSpriteJob : JobComponentSystem
{
    [BurstCompile]
    struct DrawSpriteJob : IJobForEach<TestComponent, LocalToWorld, NonUniformScale>
    {
        public float time;
        public void Execute(ref TestComponent c0, ref LocalToWorld c1, ref NonUniformScale c2)
        {
            c2.Value = new float3(1.0f + time, 1.0f + time, 1.0f);
        }
    };

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float s = math.sin((float)Time.ElapsedTime);
        var job = new DrawSpriteJob()
        {
            time = s
        }.Schedule(this, inputDeps);

        return job;
    }
}
