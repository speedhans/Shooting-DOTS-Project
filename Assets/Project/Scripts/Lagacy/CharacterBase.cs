using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Left hand point position.x = -0.045 / rot 0,-90,90
// Right hand point position.x = -0.045 / rot 0,-90,-90

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterBase : MonoBehaviour
{
    protected Animator m_Animator;
    protected Rigidbody m_Rigidbody;
    protected CapsuleCollider m_CapsuleCollider;

    [SerializeField]
    GameObject m_Model;

    protected Transform m_LeftHandPoint;
    protected Transform m_RightHandPoint;

    protected Transform m_ChestBone;
    protected Transform m_ChestTarget;
    [SerializeField]
    Vector3 m_ChestOffset;

    protected virtual void Awake()
    {
        GameObject model = Instantiate(m_Model, transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        m_Animator = GetComponentInChildren<Animator>();
        m_Animator.gameObject.AddComponent<AnimationCallbackEvent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        GameObject chesttarget = new GameObject("ChestTarget");
        m_ChestTarget = chesttarget.transform;
        m_ChestTarget.SetParent(transform);
        sunTTHelper.SetLocalTransform(m_ChestTarget, new Vector3(0.0f, 0.5f, 0.5f), Quaternion.identity);

        m_ChestBone = m_Animator.GetBoneTransform(HumanBodyBones.Chest);

        CreateGun();

        // tmp

        GameObject gun = Resources.Load<GameObject>("Weapons/Gun");
        GameObject leftgun = Instantiate(gun, m_LeftHandPoint);
        GameObject rightgutn = Instantiate(gun, m_RightHandPoint);

        sunTTHelper.SetLocalTransformIdentity(leftgun.transform);
        sunTTHelper.SetLocalTransformIdentity(rightgutn.transform);
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

    }

    protected virtual void LateUpdate()
    {
        if (!m_ChestBone || !m_ChestTarget) return;

        m_ChestBone.LookAt(m_ChestTarget);
        m_ChestBone.rotation = m_ChestBone.rotation * Quaternion.Euler(m_ChestOffset);
    }
}
