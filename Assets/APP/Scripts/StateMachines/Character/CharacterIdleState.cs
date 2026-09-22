using System;
using UnityEngine;

public class CharacterIdleState : CharacterBaseState, ITap, IPlayer
{
    private readonly int ImpactHash = Animator.StringToHash("Idle");

    public CharacterIdleState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Exit() { }
    public override void Tick(float deltaTime) { }
    public void OnTap() => stateMachine.SwitchState(new CharacterMovingState(stateMachine));

    public bool IsFalling => false;
    public Transform GetTransform() => stateMachine.GetTransform();
    public void OnCheckPoint() { }
    public void OnTakeDamage(float damage) { }
    public void OnKnockedOut() { }
}
