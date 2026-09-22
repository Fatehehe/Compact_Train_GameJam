using UnityEngine;

public class EnemyAttackState : EnemyBaseState, IEnemy
{
    private readonly int[] attackHashes =
    {
        Animator.StringToHash("Kick1"),
        Animator.StringToHash("Kick2"),
        Animator.StringToHash("Kick3"),
        Animator.StringToHash("Kick4"),
        Animator.StringToHash("Kick5")
    };

    public bool IsKnockedOut => false;
    public bool IsPushable => false;
    public bool IsSwipeable => true;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        int randomIndex = Random.Range(0, attackHashes.Length);
        stateMachine.Animator.CrossFadeInFixedTime(attackHashes[randomIndex], 0.1f);
    }

    public override void Tick(float deltaTime) { }
    public override void Exit() { }

    public void OnStopChasing()
    {
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
    }

    public void OnChasingPerformed(Vector3 position) { }
    public bool OnTakeDamage() => true;
    public void OnKnockedOut() { }
    public void OnTargetReached() { }
}