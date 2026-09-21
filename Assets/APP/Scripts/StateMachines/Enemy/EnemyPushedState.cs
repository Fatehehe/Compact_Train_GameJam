using UnityEngine;

public class EnemyPushedState : EnemyBaseState, IEnemy
{
    private readonly int FallHash = Animator.StringToHash("Fall");

    public EnemyPushedState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public bool IsKnockedOut => false;

    public bool IsPushable => false;

    public bool IsSwipeable => false;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, 0.1f);

    }

    public override void Exit()
    {

    }

    public void OnChasingPerformed(Vector3 position)
    {

    }

    public void OnKnockedOut()
    {

    }

    public void OnReturn(Vector3 position)
    {

    }

    public void OnStopChasing()
    {

    }

    public bool OnTakeDamage()
    {
        return false;
    }

    public void OnTargetReached()
    {

    }

    public override void Tick(float deltaTime)
    {

    }
}
