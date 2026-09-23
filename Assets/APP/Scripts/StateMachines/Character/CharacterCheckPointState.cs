using UnityEngine;

public class CharacterCheckPointState : CharacterBaseState, IAnimation, IPlayer
{
    private readonly int CheckPointHash = Animator.StringToHash("Run");

    public CharacterCheckPointState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(CheckPointHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        MoveHorizontal(deltaTime, stateMachine.CurrentLane);
        MoveForward(deltaTime, 5f);

        if (stateMachine.EnemyDetector.IsDetectingPushableEnemy())
        {
            stateMachine.SwitchState(new CharacterPushingState(stateMachine));
            return;
        }
    }

    public void OnFallBehindCompleted() { }
    public void OnFallCompleted() { }
    public void OnGettingUpCompleted() { }
    public void OnStandingUpCompleted() { }
    public void OnLoseAnimation() => stateMachine.SwitchState(new CharacterLoseState(stateMachine));
    public void OnWinAnimation() => stateMachine.SwitchState(new CharacterWinState(stateMachine));

    public bool IsFalling => false;
    public Transform GetTransform() => stateMachine.GetTransform();

    public void OnCheckPoint() { }
    public void OnTakeDamage(float damage) { }
    public void OnKnockedOut() { stateMachine.SwitchState(new CharacterFallState(stateMachine)); }
    public void SetPosition(Vector3 pos) => stateMachine.SetPosition(pos);
}