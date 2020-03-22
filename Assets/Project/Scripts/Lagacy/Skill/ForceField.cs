using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Skill
{
    protected override void Awake()
    {
        base.Awake();
        Initialize(SkillAction);
    }

    protected override void SkillAction()
    {
        base.SkillAction();
        Debug.Log("ForceField");
    }
}
