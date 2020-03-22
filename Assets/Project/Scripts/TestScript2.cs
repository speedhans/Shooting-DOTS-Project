using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float grayfactor = 0.0f;
    float before;
    public Color graycolor = Color.white;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalColor("GrayScaleColor", graycolor);
        Shader.SetGlobalFloat("GrayScaleFactor", grayfactor);
        if (!Application.isPlaying) return;
        if (before != grayfactor)
            GameManager.Instance.TimeScale = 1.0f - grayfactor;
        before = grayfactor;
    }
}
