using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager single;
    static public SoundManager Instance
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("SoundManager");
                single = g.AddComponent<SoundManager>();
                DontDestroyOnLoad(g);
                single.Initialize();
            }
            return single;
        }
    }

    void Initialize()
    {
        InitializeInstanceAudio();
        InitializeDefaultAudio();
    }

    GameObject m_InstanceAudioPrefab;
    GameObject m_DefaultAudioPrefab;
    List<AudioSource> m_InstanceAudioList = new List<AudioSource>();
    List<AudioSource> m_DefaultAudioList = new List<AudioSource>();

    void InitializeInstanceAudio()
    {
        m_InstanceAudioPrefab = new GameObject("Audio");
        m_InstanceAudioPrefab.transform.SetParent(this.transform);
        AudioSource audio = m_InstanceAudioPrefab.AddComponent<AudioSource>();
        audio.spatialBlend = 1.0f;
        m_InstanceAudioList.Add(audio);
        for (int i = 0; i < 9; ++i)
        {
            m_InstanceAudioList.Add(CreateInstanceAudio());
        }
    }

    AudioSource CreateInstanceAudio()
    {
        GameObject g = Instantiate(m_InstanceAudioPrefab, this.transform);
        return g.GetComponent<AudioSource>();
    }

    void InitializeDefaultAudio()
    {
        m_DefaultAudioPrefab = new GameObject("Audio");
        m_DefaultAudioPrefab.transform.SetParent(this.transform);
        AudioSource audio = m_DefaultAudioPrefab.AddComponent<AudioSource>();
        m_InstanceAudioList.Add(audio);
        for (int i = 0; i < 9; ++i)
        {
            m_DefaultAudioList.Add(CreateDefaultAudio());
        }
    }

    AudioSource CreateDefaultAudio()
    {
        GameObject g = Instantiate(m_DefaultAudioPrefab, this.transform);
        return g.GetComponent<AudioSource>();
    }

    public AudioSource PlayInstanceSound(Vector3 _AudioPosition, AudioClip _Clip)
    {
        for (int i = 0; i < m_InstanceAudioList.Count; ++i)
        {
            if (!m_InstanceAudioList[i].isPlaying)
            {
                m_InstanceAudioList[i].transform.position = _AudioPosition;
                m_InstanceAudioList[i].PlayOneShot(_Clip);
                return m_InstanceAudioList[i];
            }
        }

        AudioSource a = CreateInstanceAudio();
        a.transform.position = transform.position = _AudioPosition;
        a.PlayOneShot(_Clip);
        m_InstanceAudioList.Add(a);
        return a;
    }

    public AudioSource PlayDefaultSound(AudioClip _Clip, bool _Loop = false)
    {
        for (int i = 0; i < m_DefaultAudioList.Count; ++i)
        {
            if (!m_DefaultAudioList[i].isPlaying)
            {
                if (_Loop)
                {
                    m_DefaultAudioList[i].clip = _Clip;
                    m_DefaultAudioList[i].Play();
                }
                else
                {
                    m_DefaultAudioList[i].PlayOneShot(_Clip);
                }
                return m_DefaultAudioList[i];
            }
        }

        AudioSource a = CreateDefaultAudio();
        a.PlayOneShot(_Clip);
        m_DefaultAudioList.Add(a);
        return a;
    }

    public void SetVolume(float _Volume)
    {
        foreach (AudioSource a in m_InstanceAudioList)
        {
            a.volume = _Volume;
        }

        foreach (AudioSource a in m_DefaultAudioList)
        {
            a.volume = _Volume;
        }
    }
}
