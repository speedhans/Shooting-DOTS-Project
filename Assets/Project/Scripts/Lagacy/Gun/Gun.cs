using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sunTT;

public class Gun : MonoBehaviour
{
    [SerializeField]
    LayerMask m_TargetLayerMask;
    [SerializeField]
    Transform m_FirePoint;
    [SerializeField]
    GameObject m_FireEffectPrefab;

    float m_GunShotDelay = 0.1f;

    private void Awake()
    {
        m_FireEffectPrefab = Instantiate(m_FireEffectPrefab, m_FirePoint.transform);
        sunTTHelper.SetLocalTransformIdentity(m_FireEffectPrefab.transform);
        GunShotDisable();
    }

    private void Update()
    {
        if (!m_FireEffectPrefab.activeSelf) return;
        return;
        m_GunShotDelay -= Time.deltaTime;
        if (m_GunShotDelay <= 0.0f)
        {
            m_GunShotDelay = 0.1f;
            BulletSystem.CreateBullet(m_FirePoint.position, 1.0f, 1.0f, m_FirePoint.forward, 100.0f, m_TargetLayerMask);
        }
    }

    public void GunShotActivate()
    {
        m_FireEffectPrefab.SetActive(true);
    }

    public void GunShotDisable()
    {
        m_FireEffectPrefab.SetActive(false);
    }

    public Transform GetFirePoint() { return m_FirePoint; }
}
