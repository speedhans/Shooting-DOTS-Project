using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sunTT;

// Left hand point position.x = -0.045 / rot 0,-90,90
// Right hand point position.x = -0.045 / rot 0,-90,-90

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterBase : MonoBehaviour
{
    public const string m_AnimGunAttackKey = "GunAttackStart";
    public const string m_AnimIdleKey = "Idle";
    bool DestoryThisGameObject = false;
    public enum E_UpperBodyAnimState
    {
        IDLE,
        ATTACK,
        CAST,
    }

    public enum E_UnderBodyAnimState
    {
        LAND,
        JUMP,
        CAST,
    }

    public enum E_Live
    {
        LIVE,
        DEAD,
    }

    public E_Live m_Live = E_Live.LIVE;

    [HideInInspector]
    public E_UpperBodyAnimState m_UpperAnimState;
    [HideInInspector]
    public E_UnderBodyAnimState m_UnderAnimState;

    public Animator m_Animator { get; protected set; }
    public Rigidbody m_Rigidbody { get; protected set; }
    public CapsuleCollider m_CapsuleCollider { get; protected set; }
    public SkinnedMeshRenderer[] m_Renderers { get; protected set; }

    [SerializeField]
    GameObject m_Model;

    public Transform m_LeftHandPoint { get; protected set; }
    public Transform m_RightHandPoint { get; protected set; }

    public Transform m_ChestBone { get; protected set; }

    [HideInInspector]
    public Vector3 m_BodyPosition = new Vector3(0.0f, 0.5f, 0.0f);
    [HideInInspector]
    public Vector3 m_FacePosition = new Vector3(0.0f, 0.7f, 0.0f);

    [SerializeField]
    Vector3 m_UpperBodyAngleOffset;
    float m_UpperBodyYAngleOffsetPercentage = 1.0f;
    public float UpperBodyYAngleOffsetPercentage
    { 
        get
        {
            return m_UpperBodyYAngleOffsetPercentage;
        }
        set
        {
            m_UpperBodyYAngleOffsetPercentage = Mathf.Clamp01(value);
        }
    }
    Vector3 m_UpperBidyAddAngle;
    public float m_MaximumChestAxisX;

    protected Gun[] m_Gun = new Gun[2];

    protected List<CharacterBaseComponent> m_ComponentList = new List<CharacterBaseComponent>();

    public int m_CharacterID;
    public int m_Health;
    public int m_HealthMax;

    bool m_UseTimeScale;
    protected bool UseTimeScale
    {
        get
        {
            return m_UseTimeScale;
        }
        set
        {
            m_UseTimeScale = value;
            if (m_UseTimeScale)
                GameManager.Instance.m_TimeScaleAnimatorList.Add(m_Animator);
            else
                GameManager.Instance.m_TimeScaleAnimatorList.Remove(m_Animator);
        }
    }


    protected virtual void Awake()
    {
        m_UpperAnimState = E_UpperBodyAnimState.IDLE;
        m_UnderAnimState = E_UnderBodyAnimState.LAND;
        GameObject model = Instantiate(m_Model, transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        m_Animator = GetComponentInChildren<Animator>();
        m_Animator.gameObject.AddComponent<AnimationCallbackEvent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_ChestBone = m_Animator.GetBoneTransform(HumanBodyBones.Chest);
        m_Renderers = m_Animator.GetComponentsInChildren<SkinnedMeshRenderer>();

        CreateGun();

        // tmp

        GameObject gun = Resources.Load<GameObject>("Weapons/Gun");
        GameObject leftgun = Instantiate(gun, m_LeftHandPoint);
        GameObject rightgun = Instantiate(gun, m_RightHandPoint);

        sunTTHelper.SetLocalTransformIdentity(leftgun.transform);
        sunTTHelper.SetLocalTransformIdentity(rightgun.transform);

        m_Gun[0] = leftgun.GetComponent<Gun>();
        m_Gun[1] = rightgun.GetComponent<Gun>();
    }

    public T SetComponent<T>() where T : CharacterBaseComponent, new()
    {
        var component = new T();
        m_ComponentList.Add(component);
        return component;
    }

    public T SetComponent<T>(CharacterBase _CharacterBase) where T : CharacterBaseComponent, new()
    {
        var component = new T();
        component.Initialize(_CharacterBase);
        m_ComponentList.Add(component);
        return component;
    }

    public T FindComponent<T>() where T : CharacterBaseComponent
    {
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            if (m_ComponentList[i].GetType() == typeof(T))
            {
                return (T)m_ComponentList[i];
            }
        }

        return null;
    }

    public void RemoveComponent<T>() where T : CharacterBaseComponent
    {
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            if (m_ComponentList[i].GetType() == typeof(T))
            {
                m_ComponentList[i].DestoryComponent();
                m_ComponentList.RemoveAt(i);
                break;
            }
        }
    }

    void CreateGun()
    {
        Transform leftbone = FindBone(m_Animator.transform, "Prop_L");
        Transform rightbone = FindBone(m_Animator.transform, "Prop_R");

        GameObject leftpoint = new GameObject("LeftHandPoint");
        m_LeftHandPoint = leftpoint.transform;
        GameObject rightpoint = new GameObject("RightHandPoint");
        m_RightHandPoint = rightpoint.transform;

        m_LeftHandPoint.SetParent(leftbone);
        m_RightHandPoint.SetParent(rightbone);
        sunTTHelper.SetLocalTransform(m_LeftHandPoint, new Vector3(-0.045f, 0.0f, 0.0f), Quaternion.Euler(0.0f, -90.0f, 90.0f), Vector3.one);
        sunTTHelper.SetLocalTransform(m_RightHandPoint, new Vector3(-0.045f, 0.0f, 0.0f), Quaternion.Euler(0.0f, -90.0f, -90.0f), Vector3.one);
    }

    Transform FindBone(Transform _Transform, string _BoneName)
    {
        if (_Transform.name.Contains(_BoneName)) return _Transform;

        for (int i = 0; i < _Transform.childCount; ++i)
        {
            Transform t = FindBone(_Transform.GetChild(i), _BoneName);
            if (t)
            {
                return t;
            }
        }

        return null;
    }

    protected virtual void Update()
    {
        float deltatime = m_UseTimeScale ? Time.deltaTime * GameManager.Instance.TimeScale : Time.deltaTime;
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            m_ComponentList[i].UpdateComponent(deltatime);
        }
    }

    protected virtual void LateUpdate()
    {
        float deltatime = m_UseTimeScale ? Time.deltaTime * GameManager.Instance.TimeScale : Time.deltaTime;
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            m_ComponentList[i].LateUpdateComponent(deltatime);
        }

        if (m_Live == E_Live.DEAD || !m_ChestBone) return;

        if (m_UpperAnimState == E_UpperBodyAnimState.ATTACK)
            m_ChestBone.rotation = m_ChestBone.rotation * Quaternion.Euler(new Vector3(
                m_UpperBidyAddAngle.x + (m_UpperBodyAngleOffset.x * m_UpperBodyYAngleOffsetPercentage),
                m_UpperBidyAddAngle.y + m_UpperBodyAngleOffset.y,
                m_UpperBidyAddAngle.z + m_UpperBodyAngleOffset.z));
    }

    public Vector3 GetBodyPositionWithWorld() { return transform.position + m_BodyPosition; }
    public Vector3 GetFacePositionWithWorld() { return transform.position + m_FacePosition; }

    public void AddUpperBodyAngleX(float _AngleX) // chest 본이 돌아가 있어서 z 축으로 해야함
    {
        m_UpperBidyAddAngle.z = Mathf.Clamp(m_UpperBidyAddAngle.z += _AngleX, -m_MaximumChestAxisX, m_MaximumChestAxisX);
    }

    public void SetUpperBodyAngleX(float _AngleX) // chest 본이 돌아가 있어서 z 축으로 해야함
    {
        m_UpperBidyAddAngle.z = Mathf.Clamp(_AngleX, -m_MaximumChestAxisX, m_MaximumChestAxisX);
    }

    public Vector3 GetUpperBodyDirection() // chest 본이 돌아가 있어서 축을 교환해야 함
    {
        return (transform.rotation * Quaternion.Euler(-m_UpperBidyAddAngle.z, m_UpperBidyAddAngle.x, m_UpperBidyAddAngle.y)) * Vector3.forward;
    }

    public Vector3 GetUpperBodyAngle() // chest 본이 돌아가 있어서 축을 교환해야 함
    {
        return new Vector3(-m_UpperBidyAddAngle.z, m_UpperBidyAddAngle.x, m_UpperBidyAddAngle.y);
    }

    public Gun GetGun(int _Index) { return m_Gun[_Index]; }
    public Gun[] GetGuns() { return m_Gun; }
    public int GetGunAttachCount() { return m_Gun.Length; }

    public void SetMaterialsAlpha(float _Alpha)
    {
        foreach(SkinnedMeshRenderer s in m_Renderers)
        {
            Color color = s.material.color;
            color.a = _Alpha;
            s.material.color = color;
        }
    }

    public void GiveToDamage(int _AttackerID, int _Damage)
    {
        if (m_Live == E_Live.DEAD) return;

        m_Health -= _Damage;
        if (m_Health <= 0.0f)
        {
            m_Live = E_Live.DEAD;
            m_UpperBidyAddAngle = Vector3.zero;
        }
    }

    public void SetCastMode(bool _UpperCast = true, bool _UnderCast = true)
    {
        if (_UpperCast)
        {
            m_UpperAnimState = E_UpperBodyAnimState.CAST;
            m_Animator.CrossFade("UpperBody.Cast1", 0.1f);
            foreach (Gun g in GetGuns())
                g.GunShotDisable();
        }
        if (_UnderCast)
        {
            m_UnderAnimState = E_UnderBodyAnimState.CAST;
            m_Animator.CrossFade("UnderBody.Cast1", 0.1f);
        }
    }

    private void OnDestroy()
    {
        if (DestoryThisGameObject) return;

        foreach(CharacterBaseComponent c in m_ComponentList)
        {
            c.DestoryComponent();
        }
        m_ComponentList.Clear();
    }

    private void OnApplicationQuit()
    {
        DestoryThisGameObject = true;
    }
}
