using UnityEngine;

public class CharacterCheckPointState : CharacterBaseState
{
    private readonly int CheckPointHash = Animator.StringToHash("Run");

    public CharacterCheckPointState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("character state: CharacterCheckPointState");

        stateMachine.Animator.CrossFadeInFixedTime(CheckPointHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        HandleHorizontalMovement(deltaTime, stateMachine.CurrentLane, 0);
        MoveForward(deltaTime, 2f);
    }

    public override void Exit()
    {

    }
}
