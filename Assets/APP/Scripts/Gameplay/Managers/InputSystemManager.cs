using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InputSystemManager : IInitializable, IDisposable
{
    private PlayerInteractionService interactionService;

    [Inject]
    public void Construct(PlayerInteractionService interactionService)
    {
        this.interactionService = interactionService;
    }

    public void Initialize()
    {
        interactionService.OnSwipeUp += HandleSwipeUp;
        interactionService.OnSwipeDown += HandleSwipeDown;
        interactionService.OnSwipeLeft += HandleSwipeLeft;
        interactionService.OnSwipeRight += HandleSwipeRight;

        interactionService.OnTap += HandleTap;
        interactionService.OnLongPress += HandleLongPress;
    }

    public void Dispose()
    {
        interactionService.OnSwipeUp -= HandleSwipeUp;
        interactionService.OnSwipeDown -= HandleSwipeDown;
        interactionService.OnSwipeLeft -= HandleSwipeLeft;
        interactionService.OnSwipeRight -= HandleSwipeRight;

        interactionService.OnTap -= HandleTap;
        interactionService.OnLongPress -= HandleLongPress;
    }

    private void HandleSwipeUp()
    {
        Debug.Log("UP SWIPE detected");
    }

    private void HandleSwipeDown()
    {
        Debug.Log("DOWN SWIPE detected");
    }

    private void HandleSwipeLeft()
    {
        Debug.Log("LEFT SWIPE detected");
    }

    private void HandleSwipeRight()
    {
        Debug.Log("RIGHT SWIPE detected");
    }

    private void HandleTap(Vector2 screenPos)
    {
        Debug.Log($"TAP detected");
    }

    private void HandleLongPress(Vector2 screenPos)
    {
        Debug.Log($"LONG PRESS detected");
    }
}