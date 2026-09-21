using System;
using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine, ITap, ISwipe, IAnimation, IPlayer
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public PlayerDetector PlayerDetector { get; private set; }
    [field: SerializeField] public EnemyDetector EnemyDetector { get; private set; }

    public bool isCheckPoint = false;
    public bool isFalling = false;

    public int CurrentLane { get; set; } = 0;
    public GameConfigData Config { get; private set; }

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void Start()
    {
        SwitchState(new CharacterIdleState(this));
    }

    public void OnSwipeUp() => (currentState as ISwipe)?.OnSwipeUp();
    public void OnSwipeRight() => (currentState as ISwipe)?.OnSwipeRight();
    public void OnSwipeLeft() => (currentState as ISwipe)?.OnSwipeLeft();
    public void OnSwipeDown() => (currentState as ISwipe)?.OnSwipeDown();

    public void OnTap() => (currentState as ITap)?.OnTap();

    public void OnFallCompleted() => (currentState as IAnimation)?.OnFallCompleted();
    public void OnFallBehindCompleted() => (currentState as IAnimation)?.OnFallBehindCompleted();
    public void OnGettingUpCompleted() => (currentState as IAnimation)?.OnGettingUpCompleted();
    public void OnStandingUpCompleted() => (currentState as IAnimation)?.OnStandingUpCompleted();
    public void OnStopAnimation() => (currentState as IAnimation)?.OnStopAnimation();

    public Transform GetTransform() => transform;
    public void OnCheckPoint() => (currentState as IPlayer)?.OnCheckPoint();
    public void OnTakeDamage(float damage) => (currentState as IPlayer)?.OnTakeDamage(damage);
    public void OnKnockedOut() => (currentState as IPlayer)?.OnKnockedOut();

}