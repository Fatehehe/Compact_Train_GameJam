using System;
using UnityEngine;

public class PlayerEvents
{
    public Action OnFallCompleted;
    public Action OnFallBehindCompleted;
    public Action OnGettingUpCompleted;
    public Action OnStandingUpCompleted;
}

public static class EnemyEvents
{
    public static Action OnAttackCompleted;
    public static Action OnReturnCompleted;
}