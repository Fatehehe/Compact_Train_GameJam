using UnityEngine;

public class EnemyIdleState : EnemyBaseState, IEnemy
{
    private readonly int IdleHash = Animator.StringToHash("Idle");
    private int hp = 5;
    public bool IsKnockedOut => false;
    public bool IsPushable => true;
    public bool IsSwipeable => false;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime) { }
    public override void Exit() { }

    public void OnChasingPerformed(Vector3 position)
    {
        stateMachine.SwitchState(new EnemyMovingState(stateMachine));
    }

    public void OnTargetReached()
    {
        stateMachine.SwitchState(new EnemyAttackState(stateMachine));
    }

    public bool OnTakeDamage()
    {
        if (hp > 1)
        {
            hp--;
            return false;
        }
        stateMachine.SwitchState(new EnemyPushedState(stateMachine));

        return true;
    }

    public void OnStopChasing() { }
    public void OnKnockedOut() { }
    public void OnReturn(Vector3 position) { }
}