using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject m_FollowCameraPrefab;

    private void Awake()
    {
        GameManager.Instance.m_Main = this;
        SoundManager.Instance.SetVolume(0.3f);
    }

    public GameObject GetFollowCameraPrefab() { return m_FollowCameraPrefab; }
    public FollowCamera CreateFollowCamera()
    {
        GameObject camera = Instantiate(m_FollowCameraPrefab);
        return camera.GetComponent<FollowCamera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
