using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class PlayerAnimationService : IInitializable, IDisposable
{
    private CharacterStateMachine characterStateMachine;
    private PlayerEvents playerEvents;

    [Inject]
    public void Construct(CharacterStateMachine characterStateMachine, PlayerEvents playerEvents)
    {
        this.characterStateMachine = characterStateMachine;
        this.playerEvents = playerEvents;
    }

    public void Initialize()
    {
        playerEvents.OnFallBehindCompleted += HandleFallBehindCompleted;
        playerEvents.OnFallCompleted += HandleFallCompleted;
        playerEvents.OnGettingUpCompleted += HandleGettingUpCompleted;
        playerEvents.OnStandingUpCompleted += HandleStandingUpCompleted;
    }
    public void Dispose()
    {
        playerEvents.OnFallBehindCompleted -= HandleFallBehindCompleted;
        playerEvents.OnFallCompleted -= HandleFallCompleted;
        playerEvents.OnGettingUpCompleted -= HandleGettingUpCompleted;
        playerEvents.OnStandingUpCompleted -= HandleStandingUpCompleted;
    }

    private void HandleFallBehindCompleted() => (characterStateMachine as IAnimation)?.OnFallBehindCompleted();
    private void HandleFallCompleted() => (characterStateMachine as IAnimation)?.OnFallCompleted();
    private void HandleGettingUpCompleted() => (characterStateMachine as IAnimation)?.OnGettingUpCompleted();
    private void HandleStandingUpCompleted() => (characterStateMachine as IAnimation)?.OnStandingUpCompleted();
}
