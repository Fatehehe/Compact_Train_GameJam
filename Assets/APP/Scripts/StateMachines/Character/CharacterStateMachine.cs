using System;
using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Sense Sense { get; private set; }
    public int CurrentLane { get; set; } = 0;
    public GameConfigData Config { get; private set; }

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void OnEnable()
    {
        Sense.OnTakeDamage += HandleTakeDamage;
        Sense.OnCheckPoint += HandleCheckPoint;
    }

    void OnDisable()
    {
        Sense.OnTakeDamage -= HandleTakeDamage;
        Sense.OnCheckPoint -= HandleCheckPoint;
    }

    private void Start()
    {
        SwitchState(new CharacterIdleState(this));
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