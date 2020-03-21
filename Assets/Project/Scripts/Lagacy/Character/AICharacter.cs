using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : CharacterBase
{
    public enum E_AIState
    {
        PATROL,
        CHASE,
        ATTACK,
        ESCAPE
    }

    [SerializeField]
    protected E_AIState m_AIState = E_AIState.PATROL;
    public AIData m_AIData;
    [SerializeField]
    protected LayerMask m_TargetEnemyLayerMask;
    [SerializeField]
    protected float m_EnemySearchArea;

    [HideInInspector]
    public CharacterBase m_TargetCharacter;

    [SerializeField]
    protected NavMeshController m_NavMeshController = new NavMeshController();

    protected override void Awake()
    {
        base.Awake();

        SetComponent<AIChaseComponent>(this);
        SetComponent<AIAttackComponent>(this);
        SetComponent<AIMovementComponent>(this);
        SetComponent<AIRotateComponent>(this);
        SetComponent<AIPatrolComponent>(this);
    }

    protected override void Update()
    {
        base.Update();

        EnemyAreaSearch();

        Debug.Log(m_UpperAnimState.ToString() + " : " + m_UnderAnimState.ToString());
        Debug.Log(m_AIState);
    }

    void EnemyAreaSearch()
    {
        if (m_TargetCharacter) return;

        Collider[] coll = Physics.OverlapSphere(transform.position, m_EnemySearchArea, m_TargetEnemyLayerMask);
        if (coll == null) return;

        float distcomp = m_EnemySearchArea * 1.5f;
        for (int i = 0; i < coll.Length; ++i)
        {
            CharacterBase character = coll[i].GetComponent<CharacterBase>();
            if (character == this) return;
            float distance = Vector3.Distance(character.transform.position, transform.position);
            if (distcomp > distance)
            {
                distcomp = distance;
                m_TargetCharacter = character;
            }
        }
    }

    public E_AIState GetAIState() { return m_AIState; }
    public E_AIState SetAIState(E_AIState _State) { return m_AIState = _State; }
    public AIData GetAIData() { return m_AIData; }
    public NavMeshController GetNavMeshController() { return m_NavMeshController; }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_EnemySearchArea);
    }
#endif
}
