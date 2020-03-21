using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIData", menuName = "AICharacter/AIData", order = 0)]
public class AIData : ScriptableObject
{
    public float AttackRange;
    public float AttackDelay;
    [Range(0.1f, 1.0f)]
    public float Belligerence;
    public float EscapeDistance;
    [Range(0.0f, 1.0f)]
    public float AvoidingFatalWoundsValue;
    public float MoveSpeed;
    public float MinimumMovingRange;
    public float MaxmumMovingRange;
    public float PositionHoldingTime;
}
