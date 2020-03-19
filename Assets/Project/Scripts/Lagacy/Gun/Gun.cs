using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sunTT;

public class Gun : MonoBehaviour
{
    [SerializeField]
    AudioClip m_FireSound;
    [SerializeField]
    Transform m_FirePoint;
    [SerializeField]
    GameObject m_FireEffectPrefab;

    private void Awake()
    {
        m_FireEffectPrefab = Instantiate(m_FireEffectPrefab, m_FirePoint.transform);
        sunTTHelper.SetLocalTransformIdentity(m_FireEffectPrefab.transform);
        GunShotDisable();
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
    public AudioClip GetFireSound() { return m_FireSound; }
}
