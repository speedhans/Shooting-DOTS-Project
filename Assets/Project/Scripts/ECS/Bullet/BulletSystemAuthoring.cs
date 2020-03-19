using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystemAuthoring : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_BulletHitEffects;

    private void Awake()
    {
        BulletSystem.BulletHitPrefabs = m_BulletHitEffects;
        Destroy(this);
    }
}
