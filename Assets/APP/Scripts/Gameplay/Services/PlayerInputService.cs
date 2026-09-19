using System;
using VContainer;
using VContainer.Unity;
using UnityEngine;

public class PlayerInteractionService : IInitializable, IDisposable
{
    private readonly InputSystemService inputSystemService;
    private readonly GameConfigData config;

    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    public event Action<Vector2> OnTap;
    public event Action<Vector2> OnLongPress;

    private Vector2 startPos;
    private Vector2 currentPos;
    private float startTime;
    private bool isPressing;
    private bool hasSwiped;

    [Inject]
    public PlayerInteractionService(InputSystemService inputSystemService, GameConfigData config)
    {
        this.inputSystemService = inputSystemService;
        this.config = config;
    }

    public void Initialize()
    {
        inputSystemService?.ChangeInputState(InputStateType.Player);
        inputSystemService.OnPressStarted += HandlePressStarted;
        inputSystemService.OnPressCanceled += HandlePressEnded;
        inputSystemService.OnPressPosPerformed += HandlePosChanged;
    }

    public void Dispose()
    {
        inputSystemService.OnPressStarted -= HandlePressStarted;
        inputSystemService.OnPressCanceled -= HandlePressEnded;
        inputSystemService.OnPressPosPerformed -= HandlePosChanged;
    }

    private void HandlePosChanged(Vector2 pos)
    {
        currentPos = pos;
        if (isPressing && !hasSwiped)
        {
            Vector2 delta = currentPos - startPos;
            if (delta.magnitude >= config.SwipeMinDistance)
            {
                DetectSwipeDirection(delta);
                hasSwiped = true;
            }
        }
    }

    private void HandlePressStarted()
    {
        isPressing = true;
        hasSwiped = false;
        startPos = currentPos;
        startTime = Time.time;
    }

    private void HandlePressEnded()
    {
        if (!isPressing) return;
        isPressing = false;
        if (hasSwiped) return;

        float duration = Time.time - startTime;
        if (duration <= config.TapMaxDuration)
        {
            OnTap?.Invoke(currentPos);
        }
        else if (duration >= config.LongPressMinDuration)
        {
            OnLongPress?.Invoke(currentPos);
        }
    }

    private void DetectSwipeDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0) OnSwipeRight?.Invoke();
            else OnSwipeLeft?.Invoke();
        }
        else
        {
            if (delta.y > 0) OnSwipeUp?.Invoke();
            else OnSwipeDown?.Invoke();
        }
    }
}