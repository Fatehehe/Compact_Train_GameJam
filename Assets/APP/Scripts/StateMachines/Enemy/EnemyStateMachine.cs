using System;
using UnityEngine;
using VContainer;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Enemy Enemy { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }

    public GameConfigData Config { get; private set; }
    public CharacterStateMachine CharacterSM { get; private set; }

    public float MoveSpeed = 5f;

    [Inject]
    public void Construct(GameConfigData config, CharacterStateMachine characterSM)
    {
        this.Config = config;
        this.CharacterSM = characterSM;
    }

    private void OnEnable()
    {
        Enemy.OnTakeDamage += HandleTakeDamage;
        Enemy.OnTakeOut += HandleTakeOut;

        Targeter.OnTargetReached += HandleTargetReached;
        Targeter.OnTargetDetected += HandleTargetDetected;
    }

    void OnDisable()
    {
        Enemy.OnTakeDamage -= HandleTakeDamage;
        Enemy.OnTakeOut -= HandleTakeOut;

        Targeter.OnTargetReached -= HandleTargetReached;
        Targeter.OnTargetDetected -= HandleTargetDetected;

    }

    private void HandleTargetDetected()
    {
        if (Targeter.CurrentTarget != null) SwitchState(new EnemyMovingState(this));
    }

    private void HandleTargetReached()
    {
        Debug.Log("Target Reached");
        SwitchState(new EnemyAttackState(this));
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
