using System;
using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine, ITap, ISwipe, IAnimation, IPlayer
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public EnemyDetector EnemyDetector { get; private set; }

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

    // void OnEnable()
    // {
    //     EnemyDetector.OnAttacked += HandleAttacked;
    // }

    // void OnDisable()
    // {
    //     EnemyDetector.OnAttacked -= HandleAttacked;
    // }

    // private void HandleAttacked()
    // {
    //     SwitchState(new CharacterFallState(this));
    // }

    public void OnSwipeUp() => (currentState as ISwipe)?.OnSwipeUp();
    public void OnSwipeRight() => (currentState as ISwipe)?.OnSwipeRight();
    public void OnSwipeLeft() => (currentState as ISwipe)?.OnSwipeLeft();
    public void OnSwipeDown() => (currentState as ISwipe)?.OnSwipeDown();

    public void OnTap() => (currentState as ITap)?.OnTap();

    public void OnFallCompleted() => (currentState as IAnimation)?.OnFallCompleted();
    public void OnFallBehindCompleted() => (currentState as IAnimation)?.OnFallBehindCompleted();
    public void OnGettingUpCompleted() => (currentState as IAnimation)?.OnGettingUpCompleted();
    public void OnStandingUpCompleted() => (currentState as IAnimation)?.OnStandingUpCompleted();
    public void OnLoseAnimation() => (currentState as IAnimation)?.OnLoseAnimation();
    public void OnWinAnimation() => (currentState as IAnimation)?.OnWinAnimation();


    public bool IsFalling => (currentState as IPlayer).IsFalling;
    public Transform GetTransform() => transform;
    public void OnCheckPoint() => (currentState as IPlayer)?.OnCheckPoint();
    public void OnTakeDamage(float damage) => (currentState as IPlayer)?.OnTakeDamage(damage);
    public void OnKnockedOut() => (currentState as IPlayer)?.OnKnockedOut();

    public void SetPosition(Vector3 pos) => transform.position = pos;

}