using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, 1 << LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Collide))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * 100.0f, Color.blue);
        }
    }
}
