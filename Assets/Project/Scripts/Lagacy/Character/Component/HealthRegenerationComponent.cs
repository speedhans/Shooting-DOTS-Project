using System.Collections.Generic;
using UnityEngine;

public class HealthRegenerationComponent : CharacterBaseComponent
{
    public int m_Value = 200;
    public float m_Timer;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        m_Timer += _DeltaTime * GameManager.Instance.TimeScale;

        if (m_Timer >= 1.0f)
        {
            m_Timer = 0.0f;
            m_CharacterBase.m_Health = Mathf.Clamp(m_CharacterBase.m_Health + m_Value, 0, m_CharacterBase.m_HealthMax);
        }
    }
}
