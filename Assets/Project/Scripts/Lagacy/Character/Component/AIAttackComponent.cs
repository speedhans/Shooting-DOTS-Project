using System.Collections.Generic;
using UnityEngine;

public class AIAttackComponent : CharacterBaseComponent
{
    public float m_AttackDelay = 0.0f;
    public LayerMask m_TargetLayerMask;
    public LayerMask m_ObstacleLayerMask;
    AICharacter m_AICharacter;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AICharacter = _CharacterBase as AICharacter;
        if (!m_AICharacter) return;

        m_AttackDelay = m_AICharacter.GetAIData().AttackDelay;
        m_ObstacleLayerMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Obstacle");
        m_TargetLayerMask = 1 << LayerMask.NameToLayer("PlayerCharacter") | 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Obstacle");
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (!m_AICharacter) return;
        if (m_AICharacter.m_Live == CharacterBase.E_Live.DEAD) return;
        if (!m_AICharacter.m_TargetCharacter) return;

        if (m_AttackDelay > 0.0f)
        {
            m_AttackDelay -= _DeltaTime;
            return;
        }

        if (m_AICharacter.GetAIState() != AICharacter.E_AIState.ATTACK) return;

        if (m_AICharacter.m_UpperAnimState != CharacterBase.E_UpperBodyAnimState.ATTACK)
        {
            m_AICharacter.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.ATTACK;
            m_CharacterBase.m_Animator.CrossFade(CharacterBase.m_AnimGunAttackKey, 0.1f);
        }

        m_AttackDelay = m_AICharacter.GetAIData().AttackDelay;

        if (m_AICharacter.GetAIData().Belligerence < (1.0f - Random.Range(0.0f, 1.0f)))
        {
            m_AttackDelay += m_AICharacter.GetAIData().Belligerence * (1.0f - Random.Range(0.0f, 1.0f));
        }

        Vector3 upperbodyDir = m_AICharacter.GetUpperBodyDirection();
        Vector3 upperbodyDirNormal = upperbodyDir.normalized;
        Vector3 targetDir = m_AICharacter.m_TargetCharacter.transform.position - m_AICharacter.transform.position;
        float d = Vector3.Dot(upperbodyDirNormal, targetDir.normalized);
        if (d > 0.95f)
        {
            if (Physics.Raycast(m_AICharacter.GetBodyPositionWithWorld(), upperbodyDirNormal, targetDir.magnitude * 0.3f, m_ObstacleLayerMask))
            {
                foreach (Gun g in m_CharacterBase.GetGuns())
                    g.GunShotDisable();
                return;
            }

            foreach (Gun g in m_CharacterBase.GetGuns())
                g.GunShotActivate();
            for (int i = 0; i < m_AICharacter.GetGunAttachCount(); ++i)
            {
                BulletSystem.CreateBullet(m_AICharacter.GetGun(i).GetFirePoint().position, 1.0f,
                    m_AICharacter.m_CharacterID, E_BulletType.ORANGE, 1, upperbodyDir, 30.0f, m_TargetLayerMask, 5);
            }
            SoundManager.Instance.PlayInstanceSound(m_CharacterBase.GetGun(0).GetFirePoint().position, m_CharacterBase.GetGun(0).GetFireSound());
        }
        else
        {
            foreach (Gun g in m_CharacterBase.GetGuns())
                g.GunShotDisable();
        }
    }
}
