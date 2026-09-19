using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerControlManager : IInitializable, IDisposable
{
    private CharacterInteractionService characterInteractionService;

    [Inject]
    public void Construct(CharacterInteractionService characterInteractionService)
    {
        this.characterInteractionService = characterInteractionService;
    }

    public void Initialize()
    {
        characterInteractionService.OnSwipeUp += HandleSwipeUp;
        characterInteractionService.OnSwipeDown += HandleSwipeDown;
        characterInteractionService.OnSwipeLeft += HandleSwipeLeft;
        characterInteractionService.OnSwipeRight += HandleSwipeRight;

        characterInteractionService.OnTap += HandleTap;
        characterInteractionService.OnLongPress += HandleLongPress;
    }

    public void Dispose()
    {
        characterInteractionService.OnSwipeUp -= HandleSwipeUp;
        characterInteractionService.OnSwipeDown -= HandleSwipeDown;
        characterInteractionService.OnSwipeLeft -= HandleSwipeLeft;
        characterInteractionService.OnSwipeRight -= HandleSwipeRight;

        characterInteractionService.OnTap -= HandleTap;
        characterInteractionService.OnLongPress -= HandleLongPress;
    }

    private void HandleSwipeUp()
    {
        Debug.Log("UP SWIPE detected");
        // PlayerEvents.OnSwipeUpPerformed?.Invoke();
    }

    private void HandleSwipeDown()
    {
        Debug.Log("DOWN SWIPE detected");
        // PlayerEvents.OnSwipeDownPerformed?.Invoke();
    }

    private void HandleSwipeLeft()
    {
        Debug.Log("LEFT SWIPE detected");
        // PlayerEvents.OnSwipeLeftPerformed?.Invoke();
    }

    private void HandleSwipeRight()
    {
        Debug.Log("RIGHT SWIPE detected");
        // PlayerEvents.OnSwipeRightPerformed?.Invoke();
    }

    private void HandleTap(Vector2 screenPos)
    {
        Debug.Log($"TAP detected");
        // PlayerEvents.OnTapPerformed?.Invoke(screenPos);
    }

    private void HandleLongPress(Vector2 screenPos)
    {
        Debug.Log($"LONG PRESS detected");
        // PlayerEvents.OnLongPressPerformed?.Invoke(screenPos);
    }
}