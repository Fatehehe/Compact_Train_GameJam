using System;
using UnityEngine;

public class CharacterMovingState : CharacterBaseState, ITap, ISwipe, IPlayer, IAnimation
{
    private readonly int MovingBlendTreeHash = Animator.StringToHash("MovingBlendTree");
    private readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private readonly int VerticalHash = Animator.StringToHash("Vertical");
    private float targetVertical;

    public CharacterMovingState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        targetVertical = stateMachine.Config.minVerticalBlend;
        stateMachine.Animator.CrossFadeInFixedTime(MovingBlendTreeHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        HandleHorizontalMovement(deltaTime, stateMachine.CurrentLane, HorizontalHash);
        stateMachine.Animator.SetFloat(VerticalHash, targetVertical, stateMachine.Config.animatorDampTime, deltaTime);
        HandleVerticalMovement(deltaTime, stateMachine.Animator.GetFloat(VerticalHash));
    }

    public override void Exit() { }

    public void OnTap()
    {
        targetVertical = Mathf.Clamp(
            targetVertical + stateMachine.Config.verticalStep
            ,
            stateMachine.Config.minVerticalBlend,
            stateMachine.Config.maxVerticalBlend
        );
    }

    public void OnSwipeRight()
    {
        stateMachine.CurrentLane = Mathf.Clamp(stateMachine.CurrentLane + 1, -stateMachine.Config.maxLaneIndex, stateMachine.Config.maxLaneIndex);
    }

    public void OnSwipeLeft()
    {
        stateMachine.CurrentLane = Mathf.Clamp(stateMachine.CurrentLane - 1, -stateMachine.Config.maxLaneIndex, stateMachine.Config.maxLaneIndex);
    }

    public void OnSwipeUp() { }
    public void OnSwipeDown() { }

    public void OnCheckPoint()
    {
        stateMachine.CurrentLane = 0;
        stateMachine.SwitchState(new CharacterCheckPointState(stateMachine));
    }

    public void OnTakeDamage(float damage)
    {
        stateMachine.SwitchState(new CharacterImpactState(stateMachine, damage));
    }
    public bool IsFalling => false;
    public Transform GetTransform() => stateMachine.GetTransform();
    public void OnKnockedOut() { }

    public void OnFallCompleted() { }
    public void OnFallBehindCompleted() { }
    public void OnGettingUpCompleted() { }
    public void OnStandingUpCompleted() { }
    public void OnLoseAnimation() => stateMachine.SwitchState(new CharacterLoseState(stateMachine));
    public void OnWinAnimation() => stateMachine.SwitchState(new CharacterWinState(stateMachine));
}