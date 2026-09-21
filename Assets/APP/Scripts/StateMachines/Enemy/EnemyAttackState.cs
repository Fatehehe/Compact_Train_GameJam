using UnityEngine;

public class EnemyAttackState : EnemyBaseState, IEnemy
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private const float CrossFadeDuration = 0.1f;

    private float attackInterval = 1f;
    private float timer;

    public bool IsKnockedOut => false;
    public bool IsPushable => false;
    public bool IsSwipeable => true;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
        timer = attackInterval;
    }

    public override void Tick(float deltaTime)
    {
        timer -= deltaTime;

        if (timer <= 0f)
        {
            // TODO: Tambahkan event/panggilan logika untuk mendamage Player di sini
            // misal: EnemyInteractionService.HitPlayer();

            timer = attackInterval; // Reset timer serangan
        }
    }

    public override void Exit() { }

    public void OnChasingPerformed(Vector3 position)
    {
        stateMachine.SwitchState(new EnemyMovingState(stateMachine));
    }

    public void OnStopChasing()
    {
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
    }

    public bool OnTakeDamage() { return true; }

    public void OnKnockedOut() { }

    public void OnTargetReached() { }
}