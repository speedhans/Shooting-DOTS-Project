using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript1 : MonoBehaviour
{
    public AIData[] m_Datas;
    public GameObject m_Prefab;
    public int m_SpawnCount;
    public Transform m_SpawnPos;
    private void Start()
    {
        int l = (m_Datas.Length - 1);
        for (int i = 0; i < m_SpawnCount; ++i)
        {
            AICharacter c = Instantiate(m_Prefab, m_SpawnPos.position + new Vector3(i * 1.0f + 1.0f, 0.0f, (i % 10)), Quaternion.identity).GetComponent<AICharacter>();
            if (c)
            {
                c.m_AIData = m_Datas[i % l];
            }
        }
    }

}
