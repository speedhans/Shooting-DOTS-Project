using System.Collections.Generic;
using UnityEngine;

public class AIPatrolComponent : CharacterBaseComponent
{
    public float m_PositionHoldingTimer;

    AICharacter m_AICharacter;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AICharacter = _CharacterBase as AICharacter;
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);
        if (!m_AICharacter) return;
        if (m_AICharacter.m_TargetCharacter) return;

        if (m_AICharacter.m_UpperAnimState == CharacterBase.E_UpperBodyAnimState.ATTACK)
        {
            m_AICharacter.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.IDLE;
            m_AICharacter.m_Animator.CrossFade(CharacterBase.m_AnimIdleKey, 0.1f);
            m_AICharacter.GetNavMeshController().ClearPath();
        }

        if (m_AICharacter.GetAIState() != AICharacter.E_AIState.PATROL)
        {
            m_AICharacter.SetAIState(AICharacter.E_AIState.PATROL);
        }

        if (m_AICharacter.GetNavMeshController().IsUpdate()) return;
        if (m_PositionHoldingTimer > 0.0f)
        {
            m_PositionHoldingTimer -= _DeltaTime;
            return;
        }

        m_PositionHoldingTimer = m_AICharacter.GetAIData().PositionHoldingTime;

        float minradius = m_AICharacter.GetAIData().MinimumMovingRange;
        float maxradius = m_AICharacter.GetAIData().MaxmumMovingRange;
        float findrange = maxradius - minradius;
        Vector3 nextlocation = new Vector3(Random.Range(-findrange, findrange), 0.0f, Random.Range(-findrange, findrange));
        nextlocation.x += nextlocation.x < 0.0f ? -minradius : minradius;
        nextlocation.z += nextlocation.z < 0.0f ? -minradius : minradius;
        m_AICharacter.GetNavMeshController().SetMoveLocation(m_AICharacter.transform.position, m_AICharacter.transform.position + nextlocation);
    }
}
