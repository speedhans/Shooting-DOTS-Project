using System;
using System.Collections.Generic;

public class CharacterBaseComponent
{
    protected CharacterBase m_CharacterBase;
    public virtual void Initialize(CharacterBase _CharacterBase)
    {
        m_CharacterBase = _CharacterBase;
    }

    public virtual void UpdateComponent(float _DeltaTime)
    {

    }

    public virtual void LateUpdateComponent(float _DeltaTime)
    {

    }

    public virtual void DestoryComponent()
    {

    }
}
