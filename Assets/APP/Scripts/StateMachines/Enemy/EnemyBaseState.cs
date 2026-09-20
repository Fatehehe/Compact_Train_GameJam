using System.Collections;
using System.Collections.Generic;
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
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPosition = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPosition.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }

    protected void FaceTarget(Transform target)
    {
        Vector3 lookPosition = target.position - stateMachine.transform.position;
        lookPosition.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }

    protected void MoveToTarget(float deltaTime)
    {
        if (stateMachine.Targeter.CurrentTarget == null) { return; }
        MoveToTarget(stateMachine.Targeter.CurrentTarget.transform, deltaTime);
    }

    protected void MoveToTarget(Transform target, float deltaTime)
    {
        Vector3 direction = (target.position - stateMachine.transform.position).normalized;
        direction.y = 0f;
        Move(direction * stateMachine.MoveSpeed, deltaTime);
    }
}