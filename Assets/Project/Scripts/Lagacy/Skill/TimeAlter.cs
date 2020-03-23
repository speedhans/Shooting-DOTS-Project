using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAlter : Skill
{
    [SerializeField]
    GameObject m_CastStartEffectPrefab;
    [SerializeField]
    GameObject m_CastEndEffectPrefab;

    float m_SkillDuration = 5.0f;
    float m_SkillTimer;

    Coroutine m_Coroutine;
    protected override void Awake()
    {
        base.Awake();
        Initialize(SkillAction);
    }

    protected override void SkillAction()
    {
        base.SkillAction();

        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_CastStartEffectPrefab.name);
        if (life)
        {
            life.Initialize();
            life.transform.SetParent(m_PlayerCharacter.m_LeftHandPoint);
            sunTT.sunTTHelper.SetLocalTransform(life.transform, Vector3.zero, Quaternion.identity);
            life.gameObject.SetActive(true);
        }
        m_PlayerCharacter.SetCastMode(1, true, m_PlayerCharacter.m_UnderAnimState != CharacterBase.E_UnderBodyAnimState.JUMP);
        m_SkillTimer = m_SkillDuration;
        if (m_Coroutine != null) return;
        m_Coroutine = StartCoroutine(C_SkillEvent(1.0f));
    }

    IEnumerator C_SkillEvent(float _Delay)
    {
        yield return new WaitForSeconds(_Delay);
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_CastEndEffectPrefab.name);
        if (life)
        {
            life.Initialize();
            life.transform.position = m_PlayerCharacter.transform.position;
            life.gameObject.SetActive(true);
        }

        Shader.SetGlobalFloat("GrayScaleFactor", 1.0f);
        GameManager.Instance.TimeScale = 0.1f;
        while (m_SkillTimer > 0.0f)
        {
            m_SkillTimer -= Time.deltaTime;
            yield return null;
        }
        Shader.SetGlobalFloat("GrayScaleFactor", 0.0f);
        GameManager.Instance.TimeScale = 1.0f;

        m_Coroutine = null;
    }
}
