﻿
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
    Vector2 m_ArrowVectorInstance = Vector2.zero;
    Dictionary<KeyCode, System.Action> m_InputDownEventDic = new Dictionary<KeyCode, System.Action>();
    Dictionary<KeyCode, System.Action> m_InputUpEventDic = new Dictionary<KeyCode, System.Action>();
    Dictionary<KeyCode, System.Action> m_InputPressedEventDic = new Dictionary<KeyCode, System.Action>();
    Dictionary<KeyCode, System.Action> m_InputPressedFixedEventDic = new Dictionary<KeyCode, System.Action>();
    System.Action<Vector2, Vector2, Vector2> m_MouseMoveEvent;
    System.Action<float> m_MouseWheelEvent;

    Vector2 m_BeforeMousePosition = Vector2.zero;

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

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0.0f)
        {
            m_MouseWheelEvent?.Invoke(wheel);
        }

        Vector2 currentMousePos = Input.mousePosition;
        if (m_BeforeMousePosition != currentMousePos)
        {
            m_MouseMoveEvent?.Invoke(m_BeforeMousePosition, currentMousePos, currentMousePos - m_BeforeMousePosition);
        }
        m_BeforeMousePosition = currentMousePos;

        m_ArrowVectorInstance = Vector2.zero;

        float deltatime = Time.deltaTime;
        float increasespeed = deltatime * 10.0f;
        if (Input.GetKey(KeyCode.W))
        {
            m_ArrowVectorInstance.y += 1.0f;
            m_ArrowVector.y += increasespeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_ArrowVectorInstance.y -= 1.0f;
            m_ArrowVector.y -= increasespeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_ArrowVectorInstance.x -= 1.0f;
            m_ArrowVector.x -= increasespeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_ArrowVectorInstance.x += 1.0f;
            m_ArrowVector.x += increasespeed;
        }

        m_ArrowVector.x = Mathf.Clamp(m_ArrowVector.x, -1.0f, 1.0f);
        m_ArrowVector.y = Mathf.Clamp(m_ArrowVector.y, -1.0f, 1.0f);
    }

    private void FixedUpdate()
    {
        foreach (KeyCode k in m_InputPressedFixedEventDic.Keys)
        {
            if (Input.GetKey(k))
            {
                m_InputPressedFixedEventDic[k]?.Invoke();
            }
        }
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
    public Vector2 GetArrowVectorInstance() { return m_ArrowVectorInstance; }
    public Vector3 GetArrowVector3() { return new Vector3(m_ArrowVector.x, 0.0f, m_ArrowVector.y); }
    public Vector3 GetArrowVector3Instance() { return new Vector3(m_ArrowVectorInstance.x, 0.0f, m_ArrowVectorInstance.y); }
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

    public void AddInputPressedEvent(KeyCode _KeyCode, System.Action _Function, bool _FixedUpdateOnly = false)
    {
        System.Action action;

        if (_FixedUpdateOnly)
        {
            if (m_InputPressedFixedEventDic.TryGetValue(_KeyCode, out action))
            {
                action += _Function;
            }
            else
            {
                m_InputPressedFixedEventDic.Add(_KeyCode, new System.Action(_Function));
            }
        }
        else
        {
            if (m_InputPressedEventDic.TryGetValue(_KeyCode, out action))
            {
                action += _Function;
            }
            else
            {
                m_InputPressedEventDic.Add(_KeyCode, new System.Action(_Function));
            }
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

    /// <summary>
    /// before, after, distance
    /// </summary>
    /// <param name="_Function"></param>
    public void AddMouseMoveEvent(System.Action<Vector2, Vector2, Vector2> _Function)
    {
        m_MouseMoveEvent += _Function;
    }

    /// <summary>
    /// before, after, distance
    /// </summary>
    /// <param name="_Function"></param>
    public void ReleaseMouseMoveEvent(System.Action<Vector2, Vector2, Vector2> _Function)
    {
        m_MouseMoveEvent -= _Function;
    }

    public void AddMouseWheelEvent(System.Action<float> _Function)
    {
        m_MouseWheelEvent += _Function;
    }

    public void ReleaseMouseWheelEvent(System.Action<float> _Function)
    {
        m_MouseWheelEvent -= _Function;
    }

    private void OnApplicationQuit()
    {
        m_InputDownEventDic.Clear();
        m_InputPressedEventDic.Clear();
        m_InputUpEventDic.Clear();
        m_MouseMoveEvent = null;
        m_MouseWheelEvent = null;
    }
}
