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
        // stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FacePlayer()
    {
    }

    protected void FaceTarget(Transform target)
    {
        Vector3 lookPosition = target.position - stateMachine.transform.position;
        lookPosition.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }
}
