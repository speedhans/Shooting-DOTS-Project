using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Burst;

public enum E_BulletType
{
    ORANGE = 0,
    RED,
    GREEN,
    MAX
}

public struct BulletComponent : IComponentData
{
    public int HostID;
    public E_BulletType BulletType;
    public int Damage;
    public float3 Direction;
    public float Speed;
    public LayerMask TargetLayerMask;
    public BulletComponent(int _HostID, E_BulletType _Type, int _Damage, float3 _Direction, float _Speed, LayerMask _TargetLayerMask)
    {
        HostID = _HostID;
        BulletType = _Type;
        Damage = _Damage;
        Direction = _Direction;
        Speed = _Speed;
        TargetLayerMask = _TargetLayerMask;
    }
}

[UpdateAfter(typeof(TrailCalculateSystem))]
public class BulletSystem : ComponentSystem
{
    static public GameObject[] BulletHitPrefabs;

    static EntityManager m_EntityManger;
    static EntityArchetype m_BulletArchetype;

    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    static public void CreateBullet(float3 _StartPosition, float _LifeTime, int _HostID, E_BulletType _Type, int _Damage, float3 _Direction, float _Speed, LayerMask _TargetLayerMask, int _TrailMeshCount = 10)
    {
        int meshcount = _TrailMeshCount;

        Entity entity = m_EntityManger.CreateEntity(m_BulletArchetype);
        m_EntityManger.SetComponentData(entity, new Translation() { Value = _StartPosition });
        m_EntityManger.SetComponentData(entity, new BulletComponent(_HostID, _Type, _Damage, Unity.Mathematics.math.normalize(_Direction), _Speed, _TargetLayerMask));
        m_EntityManger.SetComponentData(entity, new TrailComponent() { MeshCount = meshcount });
        m_EntityManger.SetComponentData(entity, new LifeTimerComponent(_LifeTime));
        DynamicBuffer<TrailBufferElement> buffer = m_EntityManger.GetBuffer<TrailBufferElement>(entity);
        DynamicBuffer<float3> reinb = buffer.Reinterpret<float3>();
        for (int i = 0; i < meshcount; ++i)
        {
            reinb.Add(_StartPosition);
        }
    }

    protected override void OnCreate()
    {
        m_EntityManger = World.DefaultGameObjectInjectionWorld.EntityManager;
        m_BulletArchetype = m_EntityManger.CreateArchetype(typeof(Translation), typeof(BulletComponent), typeof(TrailBufferElement), typeof(TrailComponent), typeof(LifeTimerComponent));
        m_EndSimulationEcbSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltatime = Time.DeltaTime * GameManager.Instance.TimeScale;

        EntityCommandBuffer commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer();

        Entities.ForEach((Entity _Entity, ref BulletComponent _Bullet, ref Translation _Translation) =>
        {
            Ray ray = new Ray(_Translation.Value, _Bullet.Direction);
            RaycastHit hit;

            float length = _Bullet.Speed * deltatime;
            if (Physics.Raycast(ray, out hit, length, _Bullet.TargetLayerMask, QueryTriggerInteraction.Collide))
            {
                CharacterBase character = hit.transform.GetComponent<CharacterBase>();
                if (character)
                {
                    //Debug.Log("character hit!!");
                    _Translation.Value = hit.point;
                    commandBuffer.DestroyEntity(_Entity);
                    character.GiveToDamage(_Bullet.HostID, _Bullet.Damage);
                }
                else
                {
                    if (math.dot(-_Bullet.Direction, hit.normal) > 0.0f)
                    {
                        _Translation.Value = hit.point;
                        commandBuffer.DestroyEntity(_Entity);
                    }
                    else
                    {
                        _Translation.Value += _Bullet.Direction * length;
                        return;
                    }
                }

                LifeTimerWithObjectPool hiteffect = ObjectPool.GetObject<LifeTimerWithObjectPool>(BulletHitPrefabs[(int)_Bullet.BulletType].name);
                if (hiteffect)
                {
                    hiteffect.Initialize();
                    hiteffect.transform.position = hit.point;
                    hiteffect.gameObject.SetActive(true);
                }
            }

            _Translation.Value += _Bullet.Direction * length;
        });

    }
}
