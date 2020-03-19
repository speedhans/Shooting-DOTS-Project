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
        SoundManager.Instance.enabled = true;
        InputManager.Instance.ArrowVectorSmoothOriginRegressionRun();
    }

    public Main m_Main;
}
