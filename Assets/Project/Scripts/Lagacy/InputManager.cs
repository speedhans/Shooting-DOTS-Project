
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static InputManager single;
    static public InputManager Instance
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("InputManager");
                single = g.AddComponent<InputManager>();
                DontDestroyOnLoad(g);
            }
            return single;
        }
    }
    

    Vector2 m_ArrowVector = Vector2.zero;
    Dictionary<KeyCode, System.Action> m_InputDownEventDic = new Dictionary<KeyCode, System.Action>();
    Dictionary<KeyCode, System.Action> m_InputUpEventDic = new Dictionary<KeyCode, System.Action>();
    Dictionary<KeyCode, System.Action> m_InputPressedEventDic = new Dictionary<KeyCode, System.Action>();

    bool m_ArrowSmoothOriginRegressionRun = false;
    public float m_ArrowSmoothOriginRegressionVelocityX = 1.0f;
    public float m_ArrowSmoothOriginRegressionVelocityY = 1.0f;
    public float m_ArrowSmoothOriginRegressionTime = 0.2f;
    private void Update()
    {
        foreach (KeyCode k in m_InputDownEventDic.Keys)
        {
            if (Input.GetKeyDown(k))
            {
                m_InputDownEventDic[k]?.Invoke();
            }
        }

        foreach (KeyCode k in m_InputUpEventDic.Keys)
        {
            if (Input.GetKeyUp(k))
            {
                m_InputUpEventDic[k]?.Invoke();
            }
        }

        foreach (KeyCode k in m_InputPressedEventDic.Keys)
        {
            if (Input.GetKey(k))
            {
                m_InputPressedEventDic[k]?.Invoke();
            }
        }

        float deltatime = Time.deltaTime;
        float increasespeed = deltatime * 10.0f;
        if (Input.GetKey(KeyCode.W))
        {
            m_ArrowVector.y += increasespeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_ArrowVector.y -= increasespeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_ArrowVector.x -= increasespeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_ArrowVector.x += increasespeed;
        }

        m_ArrowVector.x = Mathf.Clamp(m_ArrowVector.x, -1.0f, 1.0f);
        m_ArrowVector.y = Mathf.Clamp(m_ArrowVector.y, -1.0f, 1.0f);
    }

    private void LateUpdate()
    {
        if (!m_ArrowSmoothOriginRegressionRun)
        {
            m_ArrowVector = Vector2.zero;
            return;
        }

        if (m_ArrowVector.x != 0.0f)
            m_ArrowVector.x = Mathf.SmoothDamp(m_ArrowVector.x, 0.0f, ref m_ArrowSmoothOriginRegressionVelocityX, m_ArrowSmoothOriginRegressionTime);
        if (m_ArrowVector.y != 0.0f)
            m_ArrowVector.y = Mathf.SmoothDamp(m_ArrowVector.y, 0.0f, ref m_ArrowSmoothOriginRegressionVelocityY, m_ArrowSmoothOriginRegressionTime);
    }

    public void ArrowVectorSmoothOriginRegressionRun() { m_ArrowSmoothOriginRegressionRun = true; }

    public void ArrowVectorSmoothOriginRegressionStop() { m_ArrowSmoothOriginRegressionRun = false; }

    public Vector2 GetArrowVector() { return m_ArrowVector; }
    public void AddInputDownEvent(KeyCode _KeyCode, System.Action _Function)
    {
        System.Action action;
        if (m_InputDownEventDic.TryGetValue(_KeyCode, out action))
        {
            action += _Function;
        }
        else
        {
            m_InputDownEventDic.Add(_KeyCode, new System.Action(_Function));
        }
    }

    public void ReleaseInputDownEvent(KeyCode _KeyCode, System.Action _Function)
    {
        System.Action action;
        if (m_InputDownEventDic.TryGetValue(_KeyCode, out action))
        {
            action -= _Function;
        }
    }

    public void AddInputUpEvent(KeyCode _KeyCode, System.Action _Function)
    {
        System.Action action;
        if (m_InputUpEventDic.TryGetValue(_KeyCode, out action))
        {
            action += _Function;
        }
        else
        {
            m_InputUpEventDic.Add(_KeyCode, new System.Action(_Function));
        }
    }

    public void ReleaseInputUpEvent(KeyCode _KeyCode, System.Action _Function)
    {
        System.Action action;
        if (m_InputUpEventDic.TryGetValue(_KeyCode, out action))
        {
            action -= _Function;
        }
    }

    public void AddInputPressedEvent(KeyCode _KeyCode, System.Action _Function)
    {
        System.Action action;
        if (m_InputPressedEventDic.TryGetValue(_KeyCode, out action))
        {
            action += _Function;
        }
        else
        {
            m_InputPressedEventDic.Add(_KeyCode, new System.Action(_Function));
        }
    }

    public void ReleaseInputPressedEvent(KeyCode _KeyCode, System.Action _Function)
    {
        System.Action action;
        if (m_InputPressedEventDic.TryGetValue(_KeyCode, out action))
        {
            action -= _Function;
        }
    }
}
