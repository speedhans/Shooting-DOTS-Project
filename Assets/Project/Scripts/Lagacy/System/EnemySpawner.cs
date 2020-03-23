using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject m_Prefab;

    AICharacter m_Target;

    [SerializeField]
    float m_SpawnDelay;
    float m_SpawnTimer;

    private IEnumerator Start()
    {
        while (GameManager.Instance.m_Main.m_GamePause) yield return null;

        GameObject g = Instantiate(m_Prefab, transform.position, Quaternion.identity);
        if (g)
        {
            m_Target = g.GetComponent<AICharacter>();
            if (m_Target == null)
            {
                Destroy(g);
                gameObject.SetActive(false);
            }
        }
        m_SpawnTimer = m_SpawnDelay;
    }

    private void Update()
    {
        if (GameManager.Instance.m_Main.m_GamePause) return;
        if (!m_Target) return;

        if (m_Target.m_Live == CharacterBase.E_Live.LIVE) return;

        m_SpawnTimer -= Time.deltaTime * GameManager.Instance.TimeScale;

        if (m_SpawnTimer <= 0.0f)
        {
            m_Target.transform.position = transform.position;
            m_Target.Revive();
            m_SpawnTimer = m_SpawnDelay;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(1,1,1));
    }
#endif
}
