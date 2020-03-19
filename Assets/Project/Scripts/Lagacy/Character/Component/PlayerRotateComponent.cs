using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateComponent : CharacterBaseComponent
{
    public float m_MouseRotateSpeedX = 50.0f;
    public float m_MouseRotateSpeedY = 50.0f;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        MouseMoveEvent(Vector2.zero, Vector2.zero, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
    }

    void MouseMoveEvent(Vector2 _Before, Vector2 _After, Vector2 _Distance)
    {
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return;

        float deltatime = Time.deltaTime;

        Vector3 beforeRot = m_CharacterBase.transform.eulerAngles;
        beforeRot.y += _Distance.x * m_MouseRotateSpeedX * deltatime;
        m_CharacterBase.transform.eulerAngles = beforeRot;
        m_CharacterBase.AddUpperBodyAngleX(_Distance.y * m_MouseRotateSpeedY * deltatime);
    }
}
