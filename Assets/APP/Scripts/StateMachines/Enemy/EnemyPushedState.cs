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
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, stateMachine.Config.crossFadeDuration);

    }

    public override void Exit() { }
    public override void Tick(float deltaTime) { }

    public void OnReturn(Vector3 position) { }
    public void OnChasingPerformed(Vector3 position) { }
    public void OnKnockedOut() { }
    public void OnStopChasing() { }
    public bool OnTakeDamage() => stateMachine.OnTakeDamage();
    public void OnTargetReached() { }

}
