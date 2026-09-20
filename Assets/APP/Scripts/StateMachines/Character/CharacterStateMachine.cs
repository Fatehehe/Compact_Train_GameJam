using System;
using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ObstacleDetector ObstacleDetector { get; private set; }
    [field: SerializeField] public EnemyDetector EnemyDetector { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }

    public bool isCheckPoint = false;
    public bool isFalling = false;


    public int CurrentLane { get; set; } = 0;
    public GameConfigData Config { get; private set; }

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void OnEnable()
    {
        ObstacleDetector.OnTakeDamage += HandleTakeDamage;
        ObstacleDetector.OnCheckPoint += HandleCheckPoint;

        EnemyDetector.OnClosestEnemyChanged += HandleClosestEnemyChanged;

        Target.OnPulled += HandlePulled;
    }

    void OnDisable()
    {
        ObstacleDetector.OnTakeDamage -= HandleTakeDamage;
        ObstacleDetector.OnCheckPoint -= HandleCheckPoint;

        EnemyDetector.OnClosestEnemyChanged -= HandleClosestEnemyChanged;

        Target.OnPulled -= HandlePulled;

    }

    private void Start()
    {
        SwitchState(new CharacterIdleState(this));
    }

    private void HandleClosestEnemyChanged(Enemy closestEnemy)
    {
        if (closestEnemy != null && !isFalling)
        {
            SwitchState(new CharacterPushingState(this));
        }
    }

    private void HandleTakeDamage(float knockBack)
    {
        if (!isFalling) SwitchState(new CharacterImpactState(this, knockBack));
    }

    private void HandleCheckPoint()
    {
        if (!isCheckPoint)
        {
            isCheckPoint = true;
            CurrentLane = 0;
            SwitchState(new CharacterCheckPointState(this));
        }
    }

    private void HandlePulled()
    {
        Debug.Log("Your caracter being pulled!");
        isFalling = true;
        SwitchState(new CharacterFallState(this));
    }
}