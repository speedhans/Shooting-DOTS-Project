﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimerWithObjectPool : MonoBehaviour
{
    public float m_LifeTime;
    float m_LifeTimer;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_LifeTimer = m_LifeTime;
    }

    private void Update()
    {
        m_LifeTimer -= Time.deltaTime * GameManager.Instance.TimeScale;
        if (m_LifeTimer <= 0.0f)
        {
            ObjectPool.PushObject(gameObject);
        }
    }
}
