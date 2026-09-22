using System;
using UnityEngine;

public class CharacterImpactState : CharacterBaseState, IAnimation, IPlayer
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
    public void OnLoseAnimation() => stateMachine.SwitchState(new CharacterLoseState(stateMachine));
    public void OnWinAnimation() => stateMachine.SwitchState(new CharacterWinState(stateMachine));


    public bool IsFalling => false;
    public Transform GetTransform() => stateMachine.GetTransform();
    public void OnCheckPoint() { }
    public void OnTakeDamage(float damage) { }
    public void OnKnockedOut() { }
    public void SetPosition(Vector3 pos) => stateMachine.SetPosition(pos);
}