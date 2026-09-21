using UnityEngine;

public class EnemyReturningState : EnemyBaseState, IEnemy
{
    private readonly int RunHash = Animator.StringToHash("Run");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 targetpos = Vector3.zero;

    public bool IsKnockedOut => false;

    public bool IsPushable => false;

    public bool IsSwipeable => false;

    public EnemyReturningState(EnemyStateMachine stateMachine, Vector3 position) : base(stateMachine)
    {
        targetpos = position;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(RunHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

        FaceSpecific(targetpos);
        MoveToSpecific(targetpos, deltaTime);

        float distSqr = (stateMachine.transform.position - targetpos).sqrMagnitude;
        if (distSqr <= 0.2f)
        {
            EnemyEvents.OnReturnCompleted?.Invoke();
        }
    }

    public override void Exit() { }

    public void OnTargetReached() { }

    public void OnStopChasing()
    {
        stateMachine.SwitchState(new EnemyIdleState(stateMachine));
    }

    public void OnChasingPerformed(Vector3 position) { }

    public bool OnTakeDamage() { return true; }

    public void OnKnockedOut() { }

    public void OnReturn(Vector3 position)
    {

    }
}