using System.Collections.Generic;
using UnityEngine;

public class AIChaseComponent : CharacterBaseComponent
{
    float m_HoldTimer;

    float m_UpdateDelayTime = 0.2f;
    float m_UpdateDelayTimeTimer;

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
        if (!m_AICharacter.m_TargetCharacter) return;
        if (m_AICharacter.m_TargetCharacter.m_Live == CharacterBase.E_Live.DEAD)
        {
            m_AICharacter.m_TargetCharacter = null;
            m_AICharacter.GetNavMeshController().ClearPath();
            foreach (Gun g in m_CharacterBase.GetGuns())
                g.GunShotDisable();
            return;
        }

        if (m_HoldTimer > 0.0f)
        {
            m_HoldTimer -= _DeltaTime;
        }

        if (m_UpdateDelayTimeTimer > 0.0f)
        {
            m_UpdateDelayTimeTimer -= _DeltaTime;
            if (m_UpdateDelayTimeTimer > 0.0f)
                return;
        }

        m_UpdateDelayTimeTimer = m_UpdateDelayTime;

        Vector3 direction = m_AICharacter.m_TargetCharacter.transform.position - m_AICharacter.transform.position;
        float distance = direction.magnitude;
        if (m_AICharacter.GetAIData().AttackRange < distance)
        {
            m_AICharacter.GetNavMeshController().SetMoveLocation(m_AICharacter.transform.position, m_AICharacter.m_TargetCharacter.transform.position);
            m_AICharacter.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.IDLE;
            m_CharacterBase.m_Animator.CrossFade(CharacterBase.m_AnimIdleKey, 0.1f);
            m_AICharacter.SetAIState(AICharacter.E_AIState.CHASE);
            foreach (Gun g in m_CharacterBase.GetGuns())
                g.GunShotDisable();
        }
        else
        {
            if (m_AICharacter.GetAIState() != AICharacter.E_AIState.ATTACK)
            {
                m_AICharacter.SetAIState(AICharacter.E_AIState.ATTACK);
                m_AICharacter.GetNavMeshController().ClearPath();
            }

            if (m_AICharacter.GetNavMeshController().IsUpdate()) return;

            if (m_HoldTimer > 0.0f) return;

            m_HoldTimer = Random.Range(m_AICharacter.GetAIData().PositionHoldingTime * 0.3f, m_AICharacter.GetAIData().PositionHoldingTime);

            float angle = m_AICharacter.transform.eulerAngles.y;
            float b = m_AICharacter.GetAIData().Belligerence;
            float offset = (distance / m_AICharacter.GetAIData().AttackRange) * b * 90.0f;
            float fixedAngle = angle + Random.Range(-offset, offset);
            angle += angle < fixedAngle ? -(100.0f - offset) : 100.0f - offset;

            if (angle > 360.0f)
                angle -= 360.0f;
            else if (angle < 0.0f)
                angle += 360.0f;


            Vector3 fwd = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
            float minrange = m_AICharacter.GetAIData().MinimumMovingRange;
            float travelDistance = Random.Range(minrange > 0.5f ? minrange : 0.5f,  m_AICharacter.GetAIData().MaxmumMovingRange);
            m_AICharacter.GetNavMeshController().SetMoveLocation(m_AICharacter.transform.position, m_AICharacter.transform.position + fwd * travelDistance);
        }
    }
}
