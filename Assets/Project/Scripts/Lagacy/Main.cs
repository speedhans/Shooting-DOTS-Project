using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject m_Tooltip;
    public Material m_GlobalAlphaMat;
    [SerializeField]
    GameObject m_FollowCameraPrefab;
    [SerializeField]
    List<AudioClip> m_BGMList;

    [HideInInspector]
    public bool m_GamePause = true;

    float m_QuitClickHoldingTime = 2.0f;
    float m_QuitClickHoldingTimer;
    private void Awake()
    {
        GameManager.Instance.m_Main = this;
        SoundManager.Instance.SetVolume(0.5f);
        SoundManager.Instance.PlayBGM(m_BGMList);

        InputManager.Instance.AddInputPressedEvent(KeyCode.Plus, VolumeUp);
        InputManager.Instance.AddInputPressedEvent(KeyCode.KeypadPlus, VolumeUp);
        InputManager.Instance.AddInputPressedEvent(KeyCode.Minus, VolumeDown);
        InputManager.Instance.AddInputPressedEvent(KeyCode.KeypadMinus, VolumeDown);
    }

    public GameObject GetFollowCameraPrefab() { return m_FollowCameraPrefab; }
    public FollowCamera CreateFollowCamera()
    {
        GameObject camera = Instantiate(m_FollowCameraPrefab);
        return camera.GetComponent<FollowCamera>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            m_QuitClickHoldingTimer += Time.unscaledDeltaTime;
            if (m_QuitClickHoldingTimer < m_QuitClickHoldingTime) return;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            m_QuitClickHoldingTimer = 0.0f;
        }

        if (m_GamePause)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                m_GamePause = false;
                m_Tooltip.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
        }

        if (!m_GamePause)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                m_Tooltip.SetActive(!m_Tooltip.activeSelf);
                if (m_Tooltip.activeSelf)
                {
                    Time.timeScale = 0.0f;
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
                else
                {
                    Time.timeScale = 1.0f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = false;
                }
            }
        }
    }

    void VolumeUp()
    {
        SoundManager.Instance.SetVolume(SoundManager.Instance.GetVolume() + 0.5f * Time.deltaTime);
    }

    void VolumeDown()
    {
        SoundManager.Instance.SetVolume(SoundManager.Instance.GetVolume() - 0.5f * Time.deltaTime);
    }
}
