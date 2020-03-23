using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Skill
{
    [SerializeField]
    GameObject m_CastStartEffectPrefab;
    [SerializeField]
    GameObject m_CastEndEffectPrefab;

    float m_SkillDuration = 5.0f;

    Coroutine m_Coroutine;

    protected override void Awake()
    {
        base.Awake();
        Initialize(SkillAction);
    }

    protected override void SkillAction()
    {
        base.SkillAction();
        Debug.Log("ForceField");

        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_CastStartEffectPrefab.name);
        if (life)
        {
            life.Initialize();
            life.transform.SetParent(m_PlayerCharacter.m_RightHandPoint);
            sunTT.sunTTHelper.SetLocalTransform(life.transform, Vector3.zero, Quaternion.identity);
            life.gameObject.SetActive(true);
        }
        m_PlayerCharacter.SetCastMode(2, true, true);
        if (m_Coroutine != null) return;
        m_Coroutine = StartCoroutine(C_SkillEvent(0.7f));
    }

    IEnumerator C_SkillEvent(float _Delay)
    {
        yield return new WaitForSeconds(_Delay);

        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_CastEndEffectPrefab.name);
        if (life)
        {
            life.m_LifeTime = m_SkillDuration;
            life.Initialize();
            life.transform.position = m_PlayerCharacter.transform.position;
            life.gameObject.SetActive(true);
        }

        m_Coroutine = null;
    }
}
