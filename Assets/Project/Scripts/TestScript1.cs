using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.ArrowVectorSmoothOriginRegressionRun();
        //InputManager.Instance.AddInputDownEvent(KeyCode.Mouse0, down);
        //InputManager.Instance.AddInputUpEvent(KeyCode.Mouse1, up);
        //InputManager.Instance.AddInputPressedEvent(KeyCode.Alpha1, pressed);
    }

    void down()
    {
        Debug.Log("down");
    }

    void up()
    {
        Debug.Log("up");
    }

    void pressed()
    {
        Debug.Log("pressed");
    }
}
