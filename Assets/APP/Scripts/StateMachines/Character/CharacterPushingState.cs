using System;
using UnityEngine;

public class CharacterPushingState : CharacterBaseState, ITap, ISwipe, IPlayer, IAnimation
{
    private readonly int PushHash = Animator.StringToHash("Pushing");

    public CharacterPushingState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PushHash, stateMachine.Config.crossFadeDuration);
    }
    public override void Exit() { }
    public override void Tick(float deltaTime) { }

    public void OnTap()
    {
        bool isEnemyKilled = stateMachine.EnemyDetector.PushActiveEnemy();
        if (isEnemyKilled)
        {
            stateMachine.SwitchState(new CharacterCheckPointState(stateMachine));
        }
    }

    public void OnSwipeUp() { }
    public void OnSwipeRight() { }
    public void OnSwipeLeft() { }
    public void OnSwipeDown() { }

    public bool IsFalling => false;
    public Transform GetTransform() => stateMachine.GetTransform();

    public void OnCheckPoint() { }
    public void OnTakeDamage(float damage) { }
    public void OnKnockedOut() { }
    public void SetPosition(Vector3 pos) => stateMachine.SetPosition(pos);

    public void OnFallCompleted() { }
    public void OnFallBehindCompleted() { }
    public void OnGettingUpCompleted() { }
    public void OnStandingUpCompleted() { }
    public void OnLoseAnimation() => stateMachine.SwitchState(new CharacterLoseState(stateMachine));
    public void OnWinAnimation() => stateMachine.SwitchState(new CharacterWinState(stateMachine));
}