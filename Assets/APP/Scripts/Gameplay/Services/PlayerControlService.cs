using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerControlService : IInitializable, IDisposable
{
    private CharacterInteractionService characterInteractionService;
    private CharacterStateMachine characterStateMachine;
    private GameplayManager gameplayManager;

    [Inject]
    public void Construct(CharacterInteractionService characterInteractionService,
    CharacterStateMachine characterStateMachine, GameplayManager gameplayManager)
    {
        this.characterInteractionService = characterInteractionService;
        this.characterStateMachine = characterStateMachine;
        this.gameplayManager = gameplayManager;
    }

    public void Initialize()
    {
        characterInteractionService.OnSwipeUp += HandleSwipeUp;
        characterInteractionService.OnSwipeDown += HandleSwipeDown;
        characterInteractionService.OnSwipeLeft += HandleSwipeLeft;
        characterInteractionService.OnSwipeRight += HandleSwipeRight;
        characterInteractionService.OnTap += HandleTap;
    }

    public void Dispose()
    {
        characterInteractionService.OnSwipeUp -= HandleSwipeUp;
        characterInteractionService.OnSwipeDown -= HandleSwipeDown;
        characterInteractionService.OnSwipeLeft -= HandleSwipeLeft;
        characterInteractionService.OnSwipeRight -= HandleSwipeRight;
        characterInteractionService.OnTap -= HandleTap;
    }

    private void HandleSwipeUp()
    {
        if (!gameplayManager.IsGameRunning) return;
        (characterStateMachine as ISwipe)?.OnSwipeUp();
        GameEvents.OnPlayerSwipe?.Invoke("Up");
    }

    private void HandleSwipeDown()
    {
        if (!gameplayManager.IsGameRunning) return;
        (characterStateMachine as ISwipe)?.OnSwipeDown();
        GameEvents.OnPlayerSwipe?.Invoke("Down");
    }

    private void HandleSwipeLeft()
    {
        if (!gameplayManager.IsGameRunning) return;
        (characterStateMachine as ISwipe)?.OnSwipeLeft();
        GameEvents.OnPlayerSwipe?.Invoke("Left");
    }

    private void HandleSwipeRight()
    {
        if (!gameplayManager.IsGameRunning) return;
        (characterStateMachine as ISwipe)?.OnSwipeRight();
        GameEvents.OnPlayerSwipe?.Invoke("Right");
    }
    private void HandleTap(Vector2 screenPos) => (characterStateMachine as ITap)?.OnTap();
}