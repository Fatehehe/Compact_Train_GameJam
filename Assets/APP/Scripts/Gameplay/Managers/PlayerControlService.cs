using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerControlService : IInitializable, IDisposable
{
    private CharacterInteractionService characterInteractionService;
    private CharacterStateMachine characterStateMachine;

    [Inject]
    public void Construct(CharacterInteractionService characterInteractionService,
    CharacterStateMachine characterStateMachine)
    {
        this.characterInteractionService = characterInteractionService;
        this.characterStateMachine = characterStateMachine;
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

    private void HandleSwipeUp() => (characterStateMachine as ISwipe)?.OnSwipeUp();
    private void HandleSwipeDown() => (characterStateMachine as ISwipe)?.OnSwipeDown();
    private void HandleSwipeLeft() => (characterStateMachine as ISwipe)?.OnSwipeLeft();
    private void HandleSwipeRight() => (characterStateMachine as ISwipe)?.OnSwipeRight();

    private void HandleTap(Vector2 screenPos) => (characterStateMachine as ITap)?.OnTap();
}