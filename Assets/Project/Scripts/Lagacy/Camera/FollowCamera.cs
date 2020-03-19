using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    Vector3 m_CameraLocalPosition;
    [SerializeField]
    Vector3 m_CameraViewDirectionOffset;
    CharacterBase m_TargetCharacter;
    bool m_TargetIsNear = false;

    Coroutine m_UpdateCoroutine;

    void Awake()
    {
        if (m_CameraLocalPosition == Vector3.zero)
        {
            m_CameraLocalPosition = new Vector3(0.0f, 0.0f, 0.01f);
        }
    }

    public void SetLocalPosition(Vector3 _Position) { m_CameraLocalPosition = _Position; }
    public void SetViewDirectionOffset(Vector3 _Offset) { m_CameraViewDirectionOffset = _Offset; }

    public void SetTarget(CharacterBase _CharacterBase) { m_TargetCharacter = _CharacterBase; }
    public void FollowRun()
    {
        if (m_UpdateCoroutine != null) StopCoroutine(m_UpdateCoroutine);
        m_UpdateCoroutine = StartCoroutine(C_TargetFollow()); 
    }

    IEnumerator C_TargetFollow()
    {
        while(true)
        {
            if (!m_TargetCharacter) yield break;
            Vector3 eyedir = m_TargetCharacter.GetUpperBodyDirection();
            Quaternion lookrot = Quaternion.LookRotation(eyedir);
            Quaternion rot = lookrot * Quaternion.Euler(m_CameraViewDirectionOffset);
            Vector3 localpos = m_CameraLocalPosition;
            float dist = (m_TargetCharacter.m_MaximumChestAxisX - Mathf.Abs(m_TargetCharacter.GetUpperBodyAngle().x)) / m_TargetCharacter.m_MaximumChestAxisX;
            localpos *= dist;
            Vector3 bodypos = m_TargetCharacter.GetBodyPositionWithWorld();
            Vector3 fixedpos = Vector3.Lerp(bodypos, m_TargetCharacter.GetFacePositionWithWorld(), dist);

            Vector3 pos = fixedpos + m_TargetCharacter.transform.rotation * localpos;
            transform.SetPositionAndRotation(pos, rot);

            if ((bodypos - transform.position).magnitude < 0.5f)
            {
                if (!m_TargetIsNear)
                {
                    m_TargetIsNear = true;
                    m_TargetCharacter.SetMaterialsAlpha(0.0f);
                }
            }
            else
            {
                if (m_TargetIsNear)
                {
                    m_TargetIsNear = false;
                    m_TargetCharacter.SetMaterialsAlpha(1.0f);
                }
            }

            yield return null;
        }
    }
}
