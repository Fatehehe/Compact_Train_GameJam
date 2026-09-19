using System;
using UnityEngine;

public static class PlayerEvents
{
    public static Action OnSwipeUpPerformed;
    public static Action OnSwipeDownPerformed;
    public static Action OnSwipeLeftPerformed;
    public static Action OnSwipeRightPerformed;
    public static Action<Vector2> OnTapPerformed;
    public static Action<Vector2> OnLongPressPerformed;
}