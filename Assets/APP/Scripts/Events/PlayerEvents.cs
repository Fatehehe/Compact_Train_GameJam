using System;
using UnityEngine;

public class PlayerEvents
{
    public Action OnSwipeUpPerformed;
    public Action OnSwipeDownPerformed;
    public Action OnSwipeLeftPerformed;
    public Action OnSwipeRightPerformed;
    public Action<Vector2> OnTapPerformed;

    public Action OnFallCompleted;
    public Action OnFallBehindCompleted;
    public Action OnGettingUpCompleted;
    public Action OnStandingUpCompleted;

}