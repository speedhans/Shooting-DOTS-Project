using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : CharacterBaseComponent
{
    public float m_MovementSpeed = 3.0f;

    int m_GroundLayer;
    int m_NumberOfJumpsAvailable = 2;
    int m_JumpingCount = 0;
    float m_JumpForce = 4.0f;
    float m_JumpTriggerDelay = 0.2f;
    float m_JumpTriggerTimer = 0.2f;
    float m_AnimationImmediatelyAfterTheJumpTimer;

    bool m_PostionUpdate = false;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);

        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Obstacle");

        InputManager.Instance.AddInputDownEvent(KeyCode.Space, Jump);
        InputManager.Instance.AddInputPressedEvent(KeyCode.W, PositionMove);
        InputManager.Instance.AddInputPressedEvent(KeyCode.S, PositionMove);
        InputManager.Instance.AddInputPressedEvent(KeyCode.A, PositionMove);
        InputManager.Instance.AddInputPressedEvent(KeyCode.D, PositionMove);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD)
        {
            if (m_CharacterBase.m_UnderAnimState == CharacterBase.E_UnderBodyAnimState.JUMP)
            {
                JumpLand();
                m_AnimationImmediatelyAfterTheJumpTimer = 0.0f;
            }
            return;
        }

        if (m_AnimationImmediatelyAfterTheJumpTimer > 0.0f)
            m_AnimationImmediatelyAfterTheJumpTimer -= _DeltaTime;

        if (m_CharacterBase.m_UnderAnimState == CharacterBase.E_UnderBodyAnimState.JUMP)
        {
            m_JumpTriggerTimer -= _DeltaTime;
            if (m_JumpTriggerTimer <= 0.0f)
            {
                float distanceToPoint = m_CharacterBase.m_CapsuleCollider.height / 2.0f - m_CharacterBase.m_CapsuleCollider.radius;
                Vector3 point1 = m_CharacterBase.transform.position + m_CharacterBase.m_CapsuleCollider.center + Vector3.up * distanceToPoint;
                Vector3 point2 = m_CharacterBase.transform.position + m_CharacterBase.m_CapsuleCollider.center - Vector3.up * distanceToPoint;
                float radius = m_CharacterBase.m_CapsuleCollider.radius * 0.95f;
                if (Physics.CapsuleCast(point1, point2, radius, Vector3.down, 0.1f, m_GroundLayer))
                {
                    JumpLand();
                }
            }
        }
        else
            SetMovementAnim(InputManager.Instance.GetArrowVector());
    }

    public override void LateUpdateComponent(float _DeltaTime)
    {
        base.LateUpdateComponent(_DeltaTime);
        m_PostionUpdate = false;
    }

    void SetMovementAnim(Vector2 _Dir)
    {
        m_CharacterBase.m_Animator.SetFloat("DirectionX", _Dir.x);
        m_CharacterBase.m_Animator.SetFloat("DirectionY", _Dir.y);

        if (m_AnimationImmediatelyAfterTheJumpTimer > 0.0f)
        {
            if (Mathf.Abs(_Dir.x) > 0.1f || Mathf.Abs(_Dir.y) > 0.1f)
            {
                m_AnimationImmediatelyAfterTheJumpTimer = 0.0f;
                AnimatorClipInfo[] infos = m_CharacterBase.m_Animator.GetCurrentAnimatorClipInfo(0);
                if (infos != null && infos.Length > 0 && (infos[0].clip.name.Contains("Fall") || infos[0].clip.name.Contains("Jump")))
                {
                    m_CharacterBase.m_Animator.CrossFade("UnderBodyTree", 0.1f);
                }
            }
        }
    }

    void PositionMove()
    {
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return;
        if (m_CharacterBase.m_UnderAnimState == CharacterBase.E_UnderBodyAnimState.JUMP) return;
        if (m_PostionUpdate) return;
        m_PostionUpdate = true;
        m_CharacterBase.transform.position += (m_CharacterBase.transform.rotation * InputManager.Instance.GetArrowVector3().normalized) * Time.deltaTime * m_MovementSpeed;
        //m_CharacterBase.m_Rigidbody.MovePosition(m_CharacterBase.transform.position + InputManager.Instance.GetArrowVector3() * Time.deltaTime * m_MovementSpeed);
    }

    void Jump()
    {
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return;
        if (m_JumpingCount >= m_NumberOfJumpsAvailable) return;

        ++m_JumpingCount;
        m_CharacterBase.m_Animator.CrossFade("Jump", 0.25f);
        m_CharacterBase.m_UnderAnimState = CharacterBase.E_UnderBodyAnimState.JUMP;
        Vector3 dir = InputManager.Instance.GetArrowVector3();
        if (Mathf.Abs(dir.x) > 0.1f || Mathf.Abs(dir.z) > 0.1f)
        {
            Vector3 speed = m_CharacterBase.transform.rotation * InputManager.Instance.GetArrowVector().normalized;
            Vector3 horizontal = (Vector3.up + new Vector3(speed.x, 0.0f, 0.0f)).normalized;
            Vector3 vertical = (Vector3.up + new Vector3(speed.z, 0.0f, 0.0f)).normalized;
            float horizontalangle = Vector3.Angle(Vector3.up, horizontal);
            float verticalangle = Vector3.Angle(Vector3.up, vertical);

            horizontalangle /= 45.0f;
            verticalangle /= 45.0f;

            float hpw = sunTT.sunTTHelper.GetTriangleDiagonalLength(horizontalangle, 1.0f);
            float vpw = sunTT.sunTTHelper.GetTriangleDiagonalLength(verticalangle, 1.0f);

            if (m_JumpingCount > 1)
            {
                hpw *= 0.3f;
                vpw *= 0.3f;
            }

            Vector3 jumpdir = new Vector3(speed.x * hpw, 1.0f, speed.z * vpw);

            m_CharacterBase.m_Rigidbody.AddForce(jumpdir * m_JumpForce, ForceMode.Impulse);
        }
        else
            m_CharacterBase.m_Rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
        m_JumpTriggerTimer = m_JumpTriggerDelay;
    }

    void JumpLand()
    {
        m_CharacterBase.m_Animator.CrossFade("Jump-Land", 0.25f);
        m_CharacterBase.m_UnderAnimState = CharacterBase.E_UnderBodyAnimState.LAND;
        m_CharacterBase.m_Rigidbody.velocity = Vector3.zero;
        m_JumpingCount = 0;
        m_AnimationImmediatelyAfterTheJumpTimer = 1.0f;
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();

        InputManager.Instance.ReleaseInputDownEvent(KeyCode.Space, Jump);
        InputManager.Instance.ReleaseInputPressedEvent(KeyCode.W, PositionMove);
        InputManager.Instance.ReleaseInputPressedEvent(KeyCode.S, PositionMove);
        InputManager.Instance.ReleaseInputPressedEvent(KeyCode.A, PositionMove);
        InputManager.Instance.ReleaseInputPressedEvent(KeyCode.D, PositionMove);
    }
}
