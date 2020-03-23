using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClip : MonoBehaviour
{
    [SerializeField]
    AudioClip[] m_Clip;
    [SerializeField]
    bool m_3D = false;
    [SerializeField]
    bool m_Loop = false;
    [SerializeField]
    bool m_AutoPlay = true;

    private void OnEnable()
    {
        if (!m_AutoPlay) return;
        if (m_Clip == null || m_Clip.Length < 0) return;
        if (m_3D)
            SoundManager.Instance.PlayInstanceSound(transform.position, m_Clip[Random.Range(0, m_Clip.Length - 1)]);
        else
            SoundManager.Instance.PlayDefaultSound(m_Clip[Random.Range(0, m_Clip.Length - 1)], m_Loop);

    }
}
