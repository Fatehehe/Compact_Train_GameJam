using UnityEngine;

public class EnemyMovingState : EnemyBaseState, IEnemy
{
    private readonly int RunHash = Animator.StringToHash("Run");

    public bool IsKnockedOut => false;
    public bool IsPushable => false;
    public bool IsSwipeable => true;

    public EnemyMovingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Collider.enabled = true;
        stateMachine.Animator.CrossFadeInFixedTime(RunHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        FaceTarget();
        MoveToTarget(deltaTime);
    }

    public override void Exit() { }

    public void OnTargetReached()
    {
        stateMachine.SwitchState(new EnemyAttackState(stateMachine));
    }

    public void OnChasingPerformed(Vector3 position) { }
    public bool OnTakeDamage() { return true; }
    public void OnKnockedOut() => stateMachine.SwitchState(new EnemyKnockedOutState(stateMachine));
}