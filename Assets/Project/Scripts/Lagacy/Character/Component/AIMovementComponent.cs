using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementComponent : CharacterBaseComponent
{
    float m_MovePerSpeed = 3.0f;
    float m_RotatePerSpeed = 360.0f;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        m_CharacterBase.GetNavMeshController().UpdateTransform(m_CharacterBase.transform, m_MovePerSpeed, m_RotatePerSpeed);
    }

    public override void LateUpdateComponent(float _DeltaTime)
    {
        base.LateUpdateComponent(_DeltaTime);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
    }
}
