using System;
using UnityEngine;

public class CharacterImpactState : CharacterBaseState, IAnimation
{
    private readonly int ImpactHash = Animator.StringToHash("Fall");
    private readonly int StandingUpHash = Animator.StringToHash("Standing Up");
    public CharacterImpactState(CharacterStateMachine stateMachine, float knockBack) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }

    public void OnFallCompleted() => stateMachine.Animator.CrossFadeInFixedTime(StandingUpHash, stateMachine.Config.crossFadeDuration);
    public void OnStandingUpCompleted() => stateMachine.SwitchState(new CharacterMovingState(stateMachine));

    public void OnFallBehindCompleted() { }
    public void OnGettingUpCompleted() { }
    public void OnStopAnimation() { }
}