using System;
using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ObstacleDetector ObstacleDetector { get; private set; }
    [field: SerializeField] public EnemyDetector EnemyDetector { get; private set; }

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
    }

    void OnDisable()
    {
        ObstacleDetector.OnTakeDamage -= HandleTakeDamage;
        ObstacleDetector.OnCheckPoint -= HandleCheckPoint;
        EnemyDetector.OnClosestEnemyChanged -= HandleClosestEnemyChanged;
    }

    private void Start()
    {
        SwitchState(new CharacterIdleState(this));
    }

    private void HandleClosestEnemyChanged(Enemy closestEnemy)
    {
        if (closestEnemy != null)
        {
            // Debug.Log($"Musuh terdekat terdeteksi: {closestEnemy.name}. Siap menyerang!");
            SwitchState(new CharacterPushingState(this));
        }
        else
        {
            SwitchState(new CharacterCheckPointState(this));
        }
    }

    private void HandleTakeDamage(float knockBack)
    {
        SwitchState(new CharacterImpactState(this, knockBack));
    }

    private void HandleCheckPoint()
    {
        SwitchState(new CharacterCheckPointState(this));
    }
}