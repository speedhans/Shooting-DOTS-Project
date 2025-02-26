﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    FollowCamera m_FollowCamera;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.m_PlayerCharacter = this;

        UseTimeScale = false;
        SetComponent<HealthRegenerationComponent>(this);
        SetComponent<PlayerMovementComponent>(this);
        SetComponent<PlayerAttackComponent>(this);
        SetComponent<PlayerRotateComponent>(this);

        m_FollowCamera = GameManager.Instance.m_Main.CreateFollowCamera();
        m_FollowCamera.SetTarget(this);
        m_FollowCamera.FollowRun();
    }
}
