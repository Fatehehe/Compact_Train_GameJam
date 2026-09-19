using UnityEngine;
using VContainer;

public class CharacterStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    public GameConfigData Config { get; private set; }

    [Inject]
    public void Construct(GameConfigData config)
    {
        this.Config = config;
    }

    private void Start()
    {
        SwitchState(new CharacterIdleState(this));
    }
}