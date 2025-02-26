﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static int MaximumFXCount = 10;

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
        GameObject g = new GameObject("BGM Audio");
        m_BGMPlayer = g.AddComponent<AudioSource>();
        m_BGMPlayer.loop = false;
        m_BGMPlayer.playOnAwake = false;
        m_BGMPlayer.Stop();

        InitializeInstanceAudio();
        InitializeDefaultAudio();
    }

    GameObject m_InstanceAudioPrefab;
    GameObject m_DefaultAudioPrefab;
    List<AudioSource> m_InstanceAudioList = new List<AudioSource>();
    List<AudioSource> m_DefaultAudioList = new List<AudioSource>();
    List<AudioClip> m_BGMList = new List<AudioClip>();
    int m_BGMNumber;
    AudioSource m_BGMPlayer;
    Coroutine m_BGMCoroutine;

    float m_Volume; 

    void InitializeInstanceAudio()
    {
        m_InstanceAudioPrefab = new GameObject("Audio");
        m_InstanceAudioPrefab.transform.SetParent(this.transform);
        AudioSource audio = m_InstanceAudioPrefab.AddComponent<AudioSource>();
        audio.spatialBlend = 1.0f;
        audio.spread = 360.0f;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 25.0f;
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

        if (m_InstanceAudioList.Count >= MaximumFXCount)
        {
            m_InstanceAudioList[0].transform.position = _AudioPosition;
            m_InstanceAudioList[0].PlayOneShot(_Clip);
            return m_InstanceAudioList[0];
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

        if (m_DefaultAudioList.Count >= MaximumFXCount)
        {
            for (int i = 0; i < m_DefaultAudioList.Count; ++i)
            {
                if (m_DefaultAudioList[i].loop != _Loop)
                {
                    m_DefaultAudioList[i].PlayOneShot(_Clip);
                    return m_DefaultAudioList[i];
                }
            }
            return null;
        }

        AudioSource a = CreateDefaultAudio();
        a.PlayOneShot(_Clip);
        m_DefaultAudioList.Add(a);
        return a;
    }

    public void PlayBGM(List<AudioClip> _Clip)
    {
        m_BGMList = _Clip;
        m_BGMNumber = 0;

        if (m_BGMCoroutine != null) StopCoroutine(m_BGMCoroutine);
        m_BGMCoroutine = StartCoroutine(C_BGMPlay(3.0f));
    }

    IEnumerator C_BGMPlay(float _WaitTime)
    {
        while(true)
        {
            if (!m_BGMPlayer.isPlaying)
            {
                yield return new WaitForSeconds(_WaitTime);
                m_BGMPlayer.clip = m_BGMList[m_BGMNumber];
                m_BGMPlayer.Play();
                ++m_BGMNumber;
                if (m_BGMNumber >= m_BGMList.Count)
                    m_BGMNumber = 0;
            }

            yield return null;
        }
    }

    public void SetVolume(float _Volume)
    {
        m_Volume = Mathf.Clamp01(_Volume);
        foreach (AudioSource a in m_InstanceAudioList)
        {
            a.volume = _Volume * 0.5f;
        }

        foreach (AudioSource a in m_DefaultAudioList)
        {
            a.volume = _Volume;
        }

        m_BGMPlayer.volume = _Volume * 0.7f;
    }

    public float GetVolume() { return m_Volume; }
}
