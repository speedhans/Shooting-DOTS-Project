using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldScaler : MonoBehaviour
{
    Vector3 m_OriginalScale = Vector3.one;
    [SerializeField]
    float m_Speed = 1.0f;

    private void Awake()
    {
        m_OriginalScale = transform.localScale;
    }

    private void OnEnable()
    {
        StartCoroutine(C_Scaling());
    }

    IEnumerator C_Scaling()
    {
        transform.localScale = m_OriginalScale * 0.1f;
        while(true)
        {
            transform.localScale += m_Speed * Vector3.one;
            if (transform.localScale.x >= m_OriginalScale.x)
            {
                transform.localScale = m_OriginalScale;
                yield break;
            }
            yield return null;
        }
    }
}
