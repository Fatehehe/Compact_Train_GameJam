using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.transform.position += motion * deltaTime;
    }

    protected void FaceTarget()
    {
        Vector3 lookPosition = stateMachine.targetPosition - stateMachine.transform.position;
        lookPosition.y = 0f;

        if (lookPosition != Vector3.zero)
            stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }

    protected void MoveToTarget(float deltaTime)
    {
        Vector3 direction = (stateMachine.targetPosition - stateMachine.transform.position).normalized;
        direction.y = 0f;
        Move(direction * stateMachine.MoveSpeed, deltaTime);
    }

    protected void MoveToSpecific(Vector3 target, float deltaTime)
    {
        Vector3 direction = (target - stateMachine.transform.position).normalized;
        direction.y = 0f;
        Move(direction * stateMachine.MoveSpeed, deltaTime);
    }

    protected void FaceSpecific(Vector3 target)
    {
        Vector3 lookPosition = target - stateMachine.transform.position;
        lookPosition.y = 0f;

        if (lookPosition != Vector3.zero)
            stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }
}