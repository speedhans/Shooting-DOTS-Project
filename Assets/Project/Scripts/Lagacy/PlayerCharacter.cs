using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    public enum E_UpperBodyAnimState
    {
        IDLE,
        ATTACK,
    }

    public enum E_UnderBodyAnimState
    {
        LAND,
        JUMP,
    }

    E_UpperBodyAnimState m_UpperAnimState = E_UpperBodyAnimState.IDLE;
    E_UnderBodyAnimState m_UnderAnimState = E_UnderBodyAnimState.LAND;

    bool m_IsDead = false;
    int m_NumberOfJumpsAvailable = 2;
    int m_JumpingCount = 0;
    float m_JumpForce = 150.0f;
    float m_JumpTriggerDelay = 0.1f;
    float m_JumpTriggerTimer = 0.1f;
    float m_AnimationImmediatelyAfterTheJumpTimer;

    protected override void Awake()
    {
        base.Awake();

        InputManager.Instance.AddInputDownEvent(KeyCode.Space, Jump);
        InputManager.Instance.AddInputPressedEvent(KeyCode.Mouse0, GunAttack);
        InputManager.Instance.AddInputUpEvent(KeyCode.Mouse0, GunAttackStop);
    }

    protected override void Update()
    {
        base.Update();

        if (!m_Animator) return;

        float deltatime = Time.deltaTime;

        TimeValueProgress(deltatime);
        SetMovementAnim(InputManager.Instance.GetArrowVector());

        if (m_UnderAnimState == E_UnderBodyAnimState.JUMP)
        {
            m_JumpTriggerTimer -= deltatime;
            if (m_JumpTriggerTimer <= 0.0f)
            {
                if (Physics.Raycast(transform.position, Vector3.down, 0.1f, 1 << LayerMask.NameToLayer("Ground")))
                {
                    JumpLand();
                }
            }
        }
    }

    void SetMovementAnim(Vector2 _Dir)
    {
        m_Animator.SetFloat("DirectionX", _Dir.x);
        m_Animator.SetFloat("DirectionY", _Dir.y);

        if (m_AnimationImmediatelyAfterTheJumpTimer > 0.0f)
        {
            if (Mathf.Abs(_Dir.x) > 0.1f || Mathf.Abs(_Dir.y) > 0.1f)
            {
                m_AnimationImmediatelyAfterTheJumpTimer = 0.0f;
                AnimatorClipInfo[] infos = m_Animator.GetCurrentAnimatorClipInfo(0);
                if (infos != null && infos.Length > 1 && infos[0].clip.name.Contains("Jump"))
                {
                    m_Animator.CrossFade("UnderBodyTree", 0.1f);
                }
            }
        }
    }

    void Jump()
    {
        if (m_JumpingCount >= m_NumberOfJumpsAvailable) return;

        ++m_JumpingCount;
        m_Animator.CrossFade("Jump", 0.25f);
        m_UnderAnimState = E_UnderBodyAnimState.JUMP;
        m_Rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Force);
        m_JumpTriggerTimer = m_JumpTriggerDelay;
    }

    void JumpLand()
    {
        m_Animator.CrossFade("Jump-Land", 0.25f);
        m_UnderAnimState = E_UnderBodyAnimState.LAND;
        m_JumpingCount = 0;
        m_AnimationImmediatelyAfterTheJumpTimer = 1.0f;
    }

    void GunAttack()
    {
        if (m_UpperAnimState != E_UpperBodyAnimState.ATTACK)
            m_Animator.CrossFade("GunAttackStart", 0.0f);
        m_UpperAnimState = E_UpperBodyAnimState.ATTACK;
        // attack

        Debug.DrawLine(m_LeftHandPoint.position, m_LeftHandPoint.position + (m_LeftHandPoint.forward * 100.0f), Color.red);
        Debug.DrawLine(m_RightHandPoint.position, m_RightHandPoint.position + (m_RightHandPoint.forward * 100.0f), Color.blue);
    }

    void GunAttackStop()
    {
        m_UpperAnimState = E_UpperBodyAnimState.IDLE;
        m_Animator.CrossFade("Idle", 0.1f);
    }

    void TimeValueProgress(float _DeltaTime)
    {
        if (m_AnimationImmediatelyAfterTheJumpTimer > 0.0f)
            m_AnimationImmediatelyAfterTheJumpTimer -= _DeltaTime;
    }
}
