using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Burst;


public struct TrailVelocityComponent : IComponentData
{
    public float3 Value;
}

public class TrailVelocitySystem : JobComponentSystem
{
    [BurstCompile]
    struct VelocityJob : IJobForEach<TrailVelocityComponent, Translation>
    {
        public float Time;
        public float DeltaTime;

        public void Execute(ref TrailVelocityComponent velocity, ref Translation translation)
        {
            var pos = translation.Value * 0.4f;
            var t = Time * 0.5f;
            var drag = velocity.Value * DeltaTime * 2.0f;
            if (math.length(velocity.Value) < math.length(velocity.Value - drag))
            {
                velocity.Value = 0;
            }
            velocity.Value -= drag;

            // ノイズを加える
            var n1 = new float3(
                noise.snoise(new float4(pos, 0 + t)),
                noise.snoise(new float4(pos, 100 + t)),
                noise.snoise(new float4(pos, 200 + t))
            );
            var n2 = new float3(
                noise.snoise(new float4(pos, 300 + t)),
                noise.snoise(new float4(pos, 400 + t)),
                noise.snoise(new float4(pos, 500 + t))
            );
            velocity.Value += math.cross(n1, n2) * DeltaTime * 40.0f;

            // 速度をもとに位置を更新
            translation.Value += velocity.Value * DeltaTime;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new VelocityJob() { DeltaTime = Time.DeltaTime, Time = (float)Time.ElapsedTime }.Schedule(this, inputDeps);
    }
}
