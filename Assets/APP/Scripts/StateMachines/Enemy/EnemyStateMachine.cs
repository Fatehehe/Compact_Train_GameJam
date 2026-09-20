using System;
using UnityEngine;
using VContainer;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Enemy Enemy { get; private set; }
    public GameConfigData Config { get; private set; }

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void OnEnable()
    {
        Enemy.OnTakeDamage += HandleTakeDamage;
        Enemy.OnTakeOut += HandleTakeOut;
    }

    void OnDisable()
    {
        Enemy.OnTakeDamage -= HandleTakeDamage;
        Enemy.OnTakeOut -= HandleTakeOut;
    }

    private void Start()
    {
        SwitchState(new EnemyIdleState(this));
    }

    private void HandleTakeDamage()
    {
        Debug.Log("Enemy took damage");
    }

    private void HandleTakeOut()
    {
        Debug.Log("Enemy is taken out");
        Destroy(gameObject);
    }
}
