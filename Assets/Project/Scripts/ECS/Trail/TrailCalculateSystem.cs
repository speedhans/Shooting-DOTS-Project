using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Burst;

public class TrailCalculateSystem : JobComponentSystem
{
    [BurstCompile]
    struct TrailJob : IJobForEachWithEntity_EBC<TrailBufferElement, Translation>
    {
        //public DynamicBuffer<TrailBufferElement> Buffer;
        //public NativeArray<Translation> Array;

        public void Execute(Entity entity, int index, DynamicBuffer<TrailBufferElement> buffer, ref Translation _Translation)
        {
            if (buffer.Length > 20)
                buffer.RemoveAt(0);
            DynamicBuffer<float3> rbuffer = buffer.Reinterpret<float3>();
            rbuffer.Add(_Translation.Value);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new TrailJob().Schedule(this, inputDeps);
    }
}
