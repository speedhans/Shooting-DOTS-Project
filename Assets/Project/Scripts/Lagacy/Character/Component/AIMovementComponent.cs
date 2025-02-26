﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementComponent : CharacterBaseComponent
{
    float m_RotatePerSpeed = 360.0f;

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
        if (m_AICharacter.m_Live == CharacterBase.E_Live.DEAD)
        {
            if (m_AICharacter.GetNavMeshController().IsUpdate())
                m_AICharacter.GetNavMeshController().ClearPath();
            return;
        }
        if (m_CharacterBase.m_UnderAnimState == CharacterBase.E_UnderBodyAnimState.JUMP)
        {

            return;
        }
        if (!m_AICharacter.GetNavMeshController().IsUpdate())
        {
            if (m_AICharacter.m_UnderAnimState == CharacterBase.E_UnderBodyAnimState.RUN)
            {
                m_AICharacter.m_Animator.SetFloat("DirectionX", 0.0f);
                m_AICharacter.m_Animator.SetFloat("DirectionY", 0.0f);
                m_AICharacter.m_UnderAnimState = CharacterBase.E_UnderBodyAnimState.LAND;
            }
            return;
        }

        if (m_AICharacter.m_UnderAnimState != CharacterBase.E_UnderBodyAnimState.RUN)
        {
            m_AICharacter.m_UnderAnimState = CharacterBase.E_UnderBodyAnimState.RUN;
            m_AICharacter.m_Animator.CrossFade(CharacterBase.m_UpperBodyTreeKey, 0.1f);
        }

        Vector3 dir = m_AICharacter.GetNavMeshController().GetCurrentPathDirection(m_CharacterBase.transform).normalized;
        m_AICharacter.m_Animator.SetFloat("DirectionX", dir.x);
        m_AICharacter.m_Animator.SetFloat("DirectionY", dir.z);
        m_AICharacter.GetNavMeshController().UpdateTransform(m_CharacterBase.transform, m_AICharacter.GetAIData().MoveSpeed, m_RotatePerSpeed, _DeltaTime);
    }

    public override void LateUpdateComponent(float _DeltaTime)
    {
        base.LateUpdateComponent(_DeltaTime);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
    }
}
