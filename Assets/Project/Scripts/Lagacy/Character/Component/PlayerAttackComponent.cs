using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackComponent : CharacterBaseComponent
{
    public float m_AttackDelay = 0.1f;
    public LayerMask m_TargetLayerMask;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);

        m_TargetLayerMask = 1 << LayerMask.NameToLayer("AICharacter") | 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Obstacle");

        //InputManager.Instance.AddInputDownEvent(KeyCode.Mouse0, AttackEnable);
        InputManager.Instance.AddInputPressedEvent(KeyCode.Mouse0, Attack);
        InputManager.Instance.AddInputUpEvent(KeyCode.Mouse0, AttackDisable);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD)
        {
            if (m_CharacterBase.m_UpperAnimState == CharacterBase.E_UpperBodyAnimState.ATTACK)
                AttackDisable();
            return;
        }

        if (m_AttackDelay > 0.0f)
        {
            m_AttackDelay -= _DeltaTime;
        }
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
    }

    void AttackEnable()
    {
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return;

        if (m_CharacterBase.m_UpperAnimState != CharacterBase.E_UpperBodyAnimState.ATTACK)
        {
            m_CharacterBase.m_Animator.CrossFade(CharacterBase.m_AnimGunAttackKey, 0.0f);
            m_CharacterBase.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.ATTACK;
            m_AttackDelay = 0.05f;
            foreach (Gun g in m_CharacterBase.GetGuns())
                g.GunShotActivate();
        }
    }

    void Attack()
    {
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return;
        if (m_CharacterBase.m_UpperAnimState == CharacterBase.E_UpperBodyAnimState.CAST) return;
        
        if (m_CharacterBase.m_UpperAnimState != CharacterBase.E_UpperBodyAnimState.ATTACK)
        {
            m_CharacterBase.m_Animator.CrossFade(CharacterBase.m_AnimGunAttackKey, 0.0f);
            m_CharacterBase.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.ATTACK;
            m_AttackDelay = 0.05f;
            foreach (Gun g in m_CharacterBase.GetGuns())
                g.GunShotActivate();
        }

        if (m_AttackDelay <= 0.0f)
        {
            m_AttackDelay = 0.15f;
            for (int i = 0; i < m_CharacterBase.GetGunAttachCount(); ++i)
            {
                SoundManager.Instance.PlayInstanceSound(m_CharacterBase.GetGun(i).GetFirePoint().position, m_CharacterBase.GetGun(i).GetFireSound());
                BulletSystem.CreateBullet(m_CharacterBase.GetGun(i).GetFirePoint().position, 1.0f,
                    m_CharacterBase.m_CharacterID, E_BulletType.ORANGE, 1, m_CharacterBase.GetUpperBodyDirection(), 30.0f, m_TargetLayerMask, 5);
            }
        }
    }

    void AttackDisable()
    {
        if (m_CharacterBase.m_UpperAnimState == CharacterBase.E_UpperBodyAnimState.CAST) return;

        m_CharacterBase.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.IDLE;
        m_CharacterBase.m_Animator.CrossFade(CharacterBase.m_AnimIdleKey, 0.1f);

        foreach (Gun g in m_CharacterBase.GetGuns())
            g.GunShotDisable();
    }
}