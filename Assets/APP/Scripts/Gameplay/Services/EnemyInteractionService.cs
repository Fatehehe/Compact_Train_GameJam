using System;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

public class EnemyInteractionService : IInitializable, IDisposable, ITickable
{
    private CharacterStateMachine characterStateMachine;
    private PlayerInteractionService playerInteractionService;

    public event Action<Vector3> OnEnemyChasingStarted;
    public event Action<Vector3> OnEnemyDetectingCharacter;


    private bool IsCheckPoint = false;

    [Inject]
    public void Construct(CharacterStateMachine characterStateMachine, PlayerInteractionService playerInteractionService)
    {
        this.characterStateMachine = characterStateMachine;
        this.playerInteractionService = playerInteractionService;
    }

    public void Initialize()
    {
        playerInteractionService.OnCheckPoint += HandleCharacterCheckPoint;
    }

    public void Dispose()
    {
        playerInteractionService.OnCheckPoint -= HandleCharacterCheckPoint;
    }

    public void Tick()
    {
        if (!IsCheckPoint) return;
        OnEnemyDetectingCharacter?.Invoke((characterStateMachine as IPlayer).GetTransform().position);
    }

    private void HandleCharacterCheckPoint()
    {
        Vector3 position = (characterStateMachine as IPlayer).GetTransform().position;
        IsCheckPoint = true;
        OnEnemyChasingStarted?.Invoke(position);
    }
}
