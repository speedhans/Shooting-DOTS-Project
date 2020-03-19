using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestScript1 : MonoBehaviour
{
    public Transform t1;
    public float l1;
    public Transform t2;
    public float l2;

    public float testAlength = 3.0f;
    public float testBlength = 4.0f;

    float glenght;
    private void Update()
    {
        if (t1 == null || t2 == null) return;
        float angle = Vector3.Angle(t1.forward, t2.forward);
        //angle = 180.0f - (90.0f + angle);
        float b = sunTT.sunTTHelper.GetTriangleDiagonalLength(angle, l1); //l1 / Mathf.Cos((angle) * (Mathf.PI / 180));
        Debug.Log(angle + " : " + b);

        glenght = b;
        //float length = Mathf.Sqrt(l1 * l1 + b * b);
        //Debug.Log(angle + " : " + A + " : " + length);
        //Debug.Log(5 * Mathf.Tan(65.0f) * Mathf.Rad2Deg);
        //Debug.Log(Mathf.Sqrt(testAlength * testAlength + testBlength * testBlength));
    }

    private void OnDrawGizmos()
    {
        if (t1 == null || t2 == null) return;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(t1.position, t1.position + (t1.forward * l1));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(t2.position, t2.position + (t2.forward * glenght));
    }
}
