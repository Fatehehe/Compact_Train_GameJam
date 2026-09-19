using System;
using UnityEngine;

public class CharacterIdleState : CharacterBaseState
{
    public CharacterIdleState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Enter Idle State");
        PlayerEvents.OnTapPerformed += HandleTap;
    }

    private void HandleTap(Vector2 vector)
    {
        Debug.Log("Handle Tap in Idle State");
        stateMachine.SwitchState(new CharacterMovingState(stateMachine));
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle State");
        PlayerEvents.OnTapPerformed -= HandleTap;
    }

    public override void Tick(float deltaTime)
    {

    }
}
