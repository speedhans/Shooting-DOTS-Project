using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Burst;

public struct BulletComponent : IComponentData
{
    public float Damage;
    public float3 Direction;
    public float Speed;
    public LayerMask TargetLayerMask;
    public BulletComponent(float _Damage, float3 _Direction, float _Speed, LayerMask _TargetLayerMask)
    {
        Damage = _Damage;
        Direction = _Direction;
        Speed = _Speed;
        TargetLayerMask = _TargetLayerMask;
    }
}

public class BulletSystem : ComponentSystem
{
    static EntityManager m_EntityManger;

    static EntityArchetype m_BulletArchetype;

    static public void CreateBullet(float3 _StartPosition, float _LifeTime, float _Damage, float3 _Direction, float _Speed, LayerMask _TargetLayerMask)
    {
        Entity entity = m_EntityManger.CreateEntity(m_BulletArchetype);
        m_EntityManger.SetComponentData(entity, new Translation() { Value = _StartPosition });
        m_EntityManger.SetComponentData(entity, new BulletComponent(_Damage, _Direction, _Speed, _TargetLayerMask));
        m_EntityManger.SetComponentData(entity, new LifeTimerComponent(_LifeTime));
        DynamicBuffer<TrailBufferElement> buffer = m_EntityManger.GetBuffer<TrailBufferElement>(entity);
        DynamicBuffer<float3> reinb = buffer.Reinterpret<float3>();
        for (int i = 0; i < 10; ++i)
        {
            reinb.Add(_StartPosition);
        }
    }

    protected override void OnCreate()
    {
        m_EntityManger = World.DefaultGameObjectInjectionWorld.EntityManager;

        m_BulletArchetype = m_EntityManger.CreateArchetype(typeof(Translation), typeof(BulletComponent), typeof(TrailBufferElement), typeof(LifeTimerComponent));
    }

    protected override void OnUpdate()
    {
        float deltatime = Time.DeltaTime;

        Entities.ForEach((Entity _Entity, ref BulletComponent _Bullet, ref Translation _Translation) =>
        {
            Ray ray = new Ray(_Translation.Value, _Bullet.Direction);
            RaycastHit hit;

            float length = _Bullet.Speed * deltatime;
            if (Physics.Raycast(ray, out hit, length, _Bullet.TargetLayerMask))
            {
                CharacterBase character = hit.transform.GetComponent<CharacterBase>();
                if (character)
                {
                    Debug.Log("character hit!!");
                    m_EntityManger.DestroyEntity(_Entity);
                }
                else
                {
                    Debug.Log("obstacle hit!!");
                    m_EntityManger.DestroyEntity(_Entity);
                }
            }
            else
            {
                _Translation.Value += _Bullet.Direction * length;
            }
        });
    }
}
