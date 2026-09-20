using System;
using UnityEngine;

public class CharacterMovingState : CharacterBaseState
{
    private readonly int MovingBlendTreeHash = Animator.StringToHash("MovingBlendTree");
    private readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private readonly int VerticalHash = Animator.StringToHash("Vertical");

    private float targetVertical;

    public CharacterMovingState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        targetVertical = stateMachine.Config.minVerticalBlend;
        PlayerEvents.OnSwipeRightPerformed += HandleSwipeRight;
        PlayerEvents.OnSwipeLeftPerformed += HandleSwipeLeft;
        PlayerEvents.OnTapPerformed += HandleTap;

        stateMachine.Animator.CrossFadeInFixedTime(MovingBlendTreeHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        HandleHorizontalMovement(deltaTime, stateMachine.CurrentLane, HorizontalHash);
        stateMachine.Animator.SetFloat(VerticalHash, targetVertical, stateMachine.Config.animatorDampTime, deltaTime);
        HandleVerticalMovement(deltaTime, stateMachine.Animator.GetFloat(VerticalHash));
    }

    public override void Exit()
    {
        PlayerEvents.OnSwipeRightPerformed -= HandleSwipeRight;
        PlayerEvents.OnSwipeLeftPerformed -= HandleSwipeLeft;
        PlayerEvents.OnTapPerformed -= HandleTap;
    }

    private void HandleTap(Vector2 vector)
    {
        targetVertical = Mathf.Clamp(
            targetVertical + stateMachine.Config.verticalStep
            ,
            stateMachine.Config.minVerticalBlend,
            stateMachine.Config.maxVerticalBlend
        );
    }

    private void HandleSwipeRight()
    {
        stateMachine.CurrentLane = Mathf.Clamp(stateMachine.CurrentLane + 1, -stateMachine.Config.maxLaneIndex, stateMachine.Config.maxLaneIndex);
    }

    private void HandleSwipeLeft()
    {
        stateMachine.CurrentLane = Mathf.Clamp(stateMachine.CurrentLane - 1, -stateMachine.Config.maxLaneIndex, stateMachine.Config.maxLaneIndex);
    }
}