using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager single;
    static public GameManager Instance
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("GameManager");
                single = g.AddComponent<GameManager>();
                DontDestroyOnLoad(g);
                single.Initialize();
            }
            return single;
        }
    }

    void Initialize()
    {
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif

        TimeScale = 1.0f;
        SoundManager.Instance.enabled = true;
        InputManager.Instance.ArrowVectorSmoothOriginRegressionRun();

        Shader.SetGlobalColor("GrayScaleColor", new Color(0.3f, 0.6f, 0.15f));
        Shader.SetGlobalFloat("GrayScaleFactor", 0.0f);
    }

    public Main m_Main;
    public PlayerCharacter m_PlayerCharacter;
    [SerializeField]
    float m_TimeScale;
    public float TimeScale
    {
        get
        {
            return m_TimeScale;
        }
        set
        {
            m_TimeScale = value;
            foreach (Animator a in m_TimeScaleAnimatorList)
            {
                a.speed = m_TimeScale;
            }
        }
    }
    [HideInInspector]
    public List<Animator> m_TimeScaleAnimatorList = new List<Animator>();
}
