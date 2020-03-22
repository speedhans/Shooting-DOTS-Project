using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbackEvent : MonoBehaviour
{
    CharacterBase m_Character;

    private void Awake()
    {
        m_Character = GetComponentInParent<CharacterBase>();
    }

    public void FootL()
    {

    }

    public void FootR()
    {

    }

    public void ShootL()
    {

    }

    public void ShootR()
    {

    }

    public void CastEnd()
    {
        m_Character.m_UpperAnimState = CharacterBase.E_UpperBodyAnimState.IDLE;
        m_Character.m_UnderAnimState = CharacterBase.E_UnderBodyAnimState.LAND;
    }
}
