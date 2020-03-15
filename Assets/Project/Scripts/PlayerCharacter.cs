using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public enum E_ANIMATIONSTATE
    {
        IDLE,
        MOVEMENT,
    }

    bool m_GunAttack = false;

    E_ANIMATIONSTATE m_AnimState = E_ANIMATIONSTATE.IDLE;

    Vector2 m_InputDirection = Vector2.zero;
    [SerializeField]
    Animator m_Animator;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            GunAttack();
        }
        else
        {
            if (m_GunAttack)
            {
                //m_Animator.SetLayerWeight(1, 0);
                m_Animator.CrossFade("GunIdle", 0.25f);
                m_GunAttack = false;
            }
        }

        m_InputDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
            m_InputDirection.y += 1.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            m_InputDirection.y -= 1.0f;
        if (Input.GetKey(KeyCode.LeftArrow))
            m_InputDirection.x -= 1.0f;
        if (Input.GetKey(KeyCode.RightArrow))
            m_InputDirection.x += 1.0f;

        if (m_InputDirection == Vector2.zero)
        {
            if (m_AnimState != E_ANIMATIONSTATE.IDLE)
            {
                m_AnimState = E_ANIMATIONSTATE.IDLE;
                m_Animator.SetTrigger("Idle");
            }
            return;
        }

        if (m_AnimState != E_ANIMATIONSTATE.MOVEMENT)
        {
            m_AnimState = E_ANIMATIONSTATE.MOVEMENT;
            m_Animator.SetTrigger("Movement");
        }

        m_InputDirection.Normalize();
        m_Animator.SetFloat("DirectionX", m_InputDirection.x);
        m_Animator.SetFloat("DirectionY", m_InputDirection.y);
    }

    public void GunAttack()
    {
        if (!m_GunAttack)
        {
            m_GunAttack = true;
            //m_Animator.SetLayerWeight(1, 1);
            m_Animator.CrossFade("GunAttackStart", 0.1f);
        }
    }
}
