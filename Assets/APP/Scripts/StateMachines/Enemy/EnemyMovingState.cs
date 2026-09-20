using UnityEngine;

public class EnemyMovingState : EnemyBaseState
{
    private readonly int RunHash = Animator.StringToHash("Run");
    private const float CrossFadeDuration = 0.1f;

    public EnemyMovingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(RunHash, CrossFadeDuration);
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        FaceTarget();
        MoveToTarget(deltaTime);
    }
}