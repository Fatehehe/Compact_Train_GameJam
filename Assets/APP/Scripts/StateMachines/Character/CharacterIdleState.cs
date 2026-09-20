using System;
using UnityEngine;

public class CharacterIdleState : CharacterBaseState
{

    public CharacterIdleState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        PlayerEvents.OnTapPerformed += HandleTap;
    }

    private void HandleTap(Vector2 vector)
    {
        stateMachine.SwitchState(new CharacterMovingState(stateMachine));
    }

    public override void Exit()
    {
        PlayerEvents.OnTapPerformed -= HandleTap;
    }

    public override void Tick(float deltaTime)
    {

    }
}
