using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunTTHelper
{
    static public void SetLocalTransformIdentity(Transform _Transform)
    {
        _Transform.localPosition = Vector3.zero;
        _Transform.localRotation = Quaternion.identity;
        _Transform.localScale = Vector3.one;
    }

    static public void SetLocalTransform(Transform _Transform, Vector3 _Position, Quaternion _Rotation, Vector3 _Scale)
    {
        _Transform.localPosition = _Position;
        _Transform.localRotation = _Rotation;
        _Transform.localScale = _Scale;
    }

    static public void SetLocalTransform(Transform _Transform, Vector3 _Position, Quaternion _Rotation)
    {
        _Transform.localPosition = _Position;
        _Transform.localRotation = _Rotation;
    }
}
