using System;
using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine, ITap, ISwipe, IAnimation, IPlayer
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public PlayerDetector PlayerDetector { get; private set; }
    [field: SerializeField] public EnemyDetector EnemyDetector { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }

    public bool isCheckPoint = false;
    public bool isFalling = false;

    public int CurrentLane { get; set; } = 0;
    public GameConfigData Config { get; private set; }

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void OnEnable()
    {
        EnemyDetector.OnClosestEnemyChanged += HandleClosestEnemyChanged;
        Target.OnPulled += HandlePulled;
    }

    void OnDisable()
    {
        EnemyDetector.OnClosestEnemyChanged -= HandleClosestEnemyChanged;
        Target.OnPulled -= HandlePulled;
    }

    private void Start()
    {
        SwitchState(new CharacterIdleState(this));
    }

    private void HandleClosestEnemyChanged(Enemy closestEnemy)
    {
        if (closestEnemy != null && !isFalling)
        {
            SwitchState(new CharacterPushingState(this));
        }
    }

    private void HandlePulled()
    {
        isFalling = true;
        SwitchState(new CharacterFallState(this));
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

    public Transform GetTransform() => transform;
    public void OnCheckPoint() => (currentState as IPlayer)?.OnCheckPoint();
    public void OnTakeDamage(float damage) => (currentState as IPlayer)?.OnTakeDamage(damage);
    public void OnKnockedOut() => (currentState as IPlayer)?.OnKnockedOut();
}