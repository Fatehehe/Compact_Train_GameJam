using UnityEngine;

public class EnemyKnockedOutState : EnemyBaseState, IEnemy
{
    private readonly int RunHash = Animator.StringToHash("Fall");

    public bool IsKnockedOut => true;
    public bool IsPushable => false;
    public bool IsSwipeable => false;
    private float knockbackSpeed = 5;
    private float knockbackDuration = 1;


    public EnemyKnockedOutState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Collider.enabled = false;
        stateMachine.Animator.CrossFadeInFixedTime(RunHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (knockbackDuration > 0f)
        {
            stateMachine.transform.Translate(Vector3.back * knockbackSpeed * deltaTime);
            knockbackSpeed = Mathf.Lerp(knockbackSpeed, 0f, deltaTime * 10f);
            knockbackDuration -= deltaTime;
        }
    }

    public override void Exit() { }

    public void OnTargetReached() { }
    public void OnChasingPerformed(Vector3 position) { }
    public bool OnTakeDamage() { return true; }
    public void OnKnockedOut() { }
}