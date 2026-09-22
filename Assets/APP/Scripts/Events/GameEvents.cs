using System;

public static class GameEvents
{
    public static Action<string> OnPlayerSwipe;
    public static Action<float> OnMissHit;
}