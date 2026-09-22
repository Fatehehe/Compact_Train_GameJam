using System;
using UnityEngine;
using VContainer;

public class EnemyStateMachine : StateMachine, IEnemy
{
    [field: SerializeField] public Animator Animator { get; private set; }
    public GameConfigData Config { get; private set; }

    public Vector3 targetPosition = Vector3.zero;
    public float MoveSpeed = 5f;

    public bool IsKnockedOut { get; private set; } = false;
    public bool IsPushable => (currentState as IEnemy).IsPushable;
    public bool IsSwipeable => (currentState as IEnemy).IsSwipeable;

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void Start()
    {
        SwitchState(new EnemyIdleState(this));
    }

    public void OnChasingPerformed(Vector3 position)
    {
        targetPosition = position;
        (currentState as IEnemy)?.OnChasingPerformed(position);
    }

    public void OnTargetReached() => (currentState as IEnemy)?.OnTargetReached();
    public void OnStopChasing() => (currentState as IEnemy)?.OnStopChasing();
    public bool OnTakeDamage() => (currentState as IEnemy).OnTakeDamage();
    public void OnReturn(Vector3 position) => (currentState as IEnemy).OnReturn(position);
    public void OnKnockedOut()
    {
        IsKnockedOut = true;
        (currentState as IEnemy)?.OnKnockedOut();
    }
}