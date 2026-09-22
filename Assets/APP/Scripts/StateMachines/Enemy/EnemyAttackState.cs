using UnityEngine;

public class EnemyAttackState : EnemyBaseState, IEnemy
{
    private readonly int AttackHash = Animator.StringToHash("Attack");

    public bool IsKnockedOut => false;
    public bool IsPushable => false;
    public bool IsSwipeable => true;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime) { }
    public override void Exit() { }


    public void OnStopChasing()
    {
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
    }
    public void OnReturn(Vector3 position)
    {
        Debug.Log("Return on state");
        stateMachine.SwitchState(new EnemyReturningState(stateMachine, position));
    }

    public void OnChasingPerformed(Vector3 position) { }
    public bool OnTakeDamage() => stateMachine.OnTakeDamage();
    public void OnKnockedOut() { }
    public void OnTargetReached() { }
}