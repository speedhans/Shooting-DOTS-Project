using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    public float PerSpeed;
}

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float2 dir = new float2(0,0);
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow))
        {
            dir += new float2(0.0f, 1.0f);
        }
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow))
        {
            dir += new float2(0.0f, -1.0f);
        }
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow))
        {
            dir += new float2(-1.0f, 0.0f);
        }
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow))
        {
            dir += new float2(1.0f, 0.0f);
        }
        float deltatime = Time.DeltaTime;
        Entities.ForEach((ref Translation _Translation, ref MovementData _MovementData) =>
        {
            float speed = deltatime * _MovementData.PerSpeed;
            _Translation.Value = new float3(_Translation.Value.x + (dir.x * speed), _Translation.Value.y + (dir.y * speed), _Translation.Value.z);
        });
    }
}
