using System;
using System.Collections.Generic;
using UnityEngine;

public class AIRotateComponent :CharacterBaseComponent
{
    public float m_RotatePerSpeed = 360.0f;

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
        if (m_AICharacter.m_Live == CharacterBase.E_Live.DEAD) return;

        float rotValue = m_RotatePerSpeed * _DeltaTime;
        Vector3 rot = m_AICharacter.transform.eulerAngles;
        Vector3 focusingdir = Vector3.zero;

        if (m_AICharacter.m_TargetCharacter)
        {
            Vector3 dir = (m_AICharacter.m_TargetCharacter.transform.position - m_AICharacter.transform.position).normalized;
            Vector3 cdir = dir;
            cdir.y = 0.0f;
            float dirAngle = Vector3.Angle(cdir, dir);

            m_AICharacter.SetUpperBodyAngleX(dir.y < 0.0f ? -dirAngle : dirAngle);

            focusingdir = (m_AICharacter.m_TargetCharacter.transform.position - m_AICharacter.transform.position).normalized;
        }
        else if (m_AICharacter.GetNavMeshController().IsUpdate())
        {
            focusingdir = m_AICharacter.GetNavMeshController().GetCurrentPathDirection(m_AICharacter.transform).normalized;
        }

        if (focusingdir == Vector3.zero) return;

        float dot = Vector3.Dot(m_AICharacter.transform.right, focusingdir);
        Vector3 dirRot = Quaternion.LookRotation(focusingdir).eulerAngles;
        if (Mathf.Abs(dirRot.y - rot.y) > rotValue)
        {
            if (dot < 0.0f)
                rot.y -= rotValue;
            else
                rot.y += rotValue;
        }
        else
        {
            rot.y = dirRot.y;
        }
        rot.y = rot.y % 360.0f;

        m_AICharacter.transform.eulerAngles = rot;
    }
}
