using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript1 : MonoBehaviour
{
    public GameObject[] m_Prefab;
    public int m_SpawnCount;
    public Transform m_SpawnPos;
    private void Start()
    {
        int l = (m_Prefab.Length);
        for (int i = 0; i < m_SpawnCount; ++i)
        {
            AICharacter c = Instantiate(m_Prefab[i % l], m_SpawnPos.position + new Vector3(i * 1.0f + 1.0f, 0.0f, (i % 10)), Quaternion.identity).GetComponent<AICharacter>();
        }
    }

}
