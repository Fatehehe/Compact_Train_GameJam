using UnityEngine;

public class CharacterCheckPointState : CharacterBaseState, IAnimation
{
    private readonly int CheckPointHash = Animator.StringToHash("Run");
    public CharacterCheckPointState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(CheckPointHash, stateMachine.Config.crossFadeDuration);
    }
    public override void Exit() { }

    public void OnFallBehindCompleted() { }
    public void OnFallCompleted() { }
    public void OnGettingUpCompleted() { }
    public void OnStandingUpCompleted() { }
    public void OnStopAnimation() => stateMachine.SwitchState(new CharacterIdleState(stateMachine));

    public override void Tick(float deltaTime)
    {
        HandleHorizontalMovement(deltaTime, stateMachine.CurrentLane, 0);
        MoveForward(deltaTime, 5f);
    }

}
