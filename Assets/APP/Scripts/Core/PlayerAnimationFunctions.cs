using System;
using UnityEngine;
using VContainer;

public class PlayerAnimationFunctions : MonoBehaviour
{
    private PlayerEvents playerEvents;

    [Inject]
    public void Construct(PlayerEvents playerEvents)
    {
        this.playerEvents = playerEvents;
    }
    public void FallCompletedFunction()
    {
        playerEvents.OnFallCompleted?.Invoke();
    }

    public void FallBehindCompletedFunction()
    {
        playerEvents.OnFallBehindCompleted?.Invoke();
    }

    public void GettingUpCompleted()
    {
        playerEvents.OnGettingUpCompleted?.Invoke();
    }

    public void StandingUpCompleted()
    {
        playerEvents.OnStandingUpCompleted?.Invoke();
    }
}