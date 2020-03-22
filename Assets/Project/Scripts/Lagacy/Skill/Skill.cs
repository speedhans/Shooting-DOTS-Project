using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Range(0,9)]
    [SerializeField]
    protected int m_Index;
    [SerializeField]
    protected float m_Cooldown;
    protected float m_CooldownTimer;

    protected PlayerCharacter m_PlayerCharacter;
    protected UnityEngine.UI.Image m_Fill;
    System.Action m_SkillEvent;

    protected void Initialize(System.Action _SkillEvent)
    {
        m_SkillEvent += _SkillEvent;
    }

    protected virtual void Awake()
    {
        m_Fill = transform.Find("Fill").GetComponent<UnityEngine.UI.Image>();
        InputManager.Instance.AddInputDownEvent((KeyCode)(m_Index + 48), SkillAction); // 48 ~57
    }

    IEnumerator Start()
    {
        while(m_PlayerCharacter == null)
        {
            m_PlayerCharacter = GameManager.Instance.m_PlayerCharacter;
            yield return null;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!m_PlayerCharacter) return;
        
        if (m_CooldownTimer > 0.0f)
        {
            m_CooldownTimer -= Time.deltaTime;
            if (m_CooldownTimer < 0.0f)
                m_CooldownTimer = 0.0f;
        }

        m_Fill.fillAmount = m_CooldownTimer / m_Cooldown;
    }

    protected virtual void SkillAction()
    {
        if (m_PlayerCharacter.m_UpperAnimState == CharacterBase.E_UpperBodyAnimState.CAST) return;
        if (m_CooldownTimer > 0.0f) return;
        m_CooldownTimer = m_Cooldown;
        Debug.Log("Use Skill " + m_Index.ToString());

        m_SkillEvent?.Invoke();
    }
}
