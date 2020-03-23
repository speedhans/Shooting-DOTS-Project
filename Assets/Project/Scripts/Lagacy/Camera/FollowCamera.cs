using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    struct S_OBSTACLEDATA
    {
        public Material m_BeforeMat;
        public MeshRenderer m_MeshRenderer;
        public Collider m_Collider;
    }

    bool DestoryThisGameObject = false;

    float m_CameraDistanceOffset = 1.0f;
    [SerializeField]
    Vector3 m_CameraLocalPosition;
    [SerializeField]
    Vector3 m_CameraViewDirectionOffset;
    //Vector3 m_CameraViewLocalPosition;
    CharacterBase m_TargetCharacter;
    bool m_TargetIsNear = false;

    Coroutine m_UpdateCoroutine;

    Ray m_Ray = new Ray();
    int m_ObstacleLayer = 0;

    List<S_OBSTACLEDATA> m_ObstacleList = new List<S_OBSTACLEDATA>();

    void Awake()
    {
        InputManager.Instance.AddMouseWheelEvent(MouseWheelEvent);

        if (m_CameraLocalPosition == Vector3.zero)
        {
            m_CameraLocalPosition = new Vector3(0.0f, 0.0f, 0.01f);
        }

        m_ObstacleLayer = 1 << LayerMask.NameToLayer("Obstacle");
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
            Vector3 localpos = m_CameraLocalPosition * m_CameraDistanceOffset;
            float dist = (m_TargetCharacter.m_MaximumChestAxisX - Mathf.Abs(m_TargetCharacter.GetUpperBodyAngle().x)) / m_TargetCharacter.m_MaximumChestAxisX;
            localpos *= dist;
            Vector3 bodypos = m_TargetCharacter.GetBodyPositionWithWorld();
            Vector3 fixedpos = Vector3.Lerp(bodypos, m_TargetCharacter.GetFacePositionWithWorld(), dist);

            Vector3 pos = fixedpos + m_TargetCharacter.transform.rotation * localpos;
            transform.SetPositionAndRotation(pos, rot);

            float distanceToTarget = (bodypos - transform.position).magnitude;

            if (distanceToTarget < 0.5f)
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

            m_Ray.origin = transform.position;
            m_Ray.direction = transform.forward;


            // 카메라 전방 장애물 체크
            RaycastHit[] colls = Physics.RaycastAll(m_Ray, distanceToTarget * 0.99f, m_ObstacleLayer);

            List<S_OBSTACLEDATA> newlist = new List<S_OBSTACLEDATA>();

            for (int i = 0; i < m_ObstacleList.Count; ++i)
            {
                m_ObstacleList[i].m_MeshRenderer.material = m_ObstacleList[i].m_BeforeMat;
            }
            m_ObstacleList.Clear();
            for (int i = 0; i < colls.Length; ++i)
            {
                S_OBSTACLEDATA data;
                data.m_MeshRenderer = colls[i].transform.GetComponent<MeshRenderer>();
                data.m_BeforeMat = data.m_MeshRenderer.material;
                data.m_MeshRenderer.material = GameManager.Instance.m_Main.m_GlobalAlphaMat;
                data.m_Collider = colls[i].collider;
                m_ObstacleList.Add(data);
            }

            yield return null;
        }
    }

    void MouseWheelEvent(float _Value)
    {
        m_CameraDistanceOffset = Mathf.Clamp(m_CameraDistanceOffset + _Value, 0.1f, 1.0f);
    }

    private void OnDestroy()
    {
        if (DestoryThisGameObject) return;
        InputManager.Instance.ReleaseMouseWheelEvent(MouseWheelEvent);
    }

    private void OnApplicationQuit()
    {
        DestoryThisGameObject = true;
    }
}
