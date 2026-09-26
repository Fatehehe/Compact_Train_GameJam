using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerInteractionService : IInitializable, IDisposable
{
    private PlayerDetector playerDetector;
    private CharacterStateMachine characterStateMachine;

    public event Action OnCheckPoint;
    public event Action<float> OnTakeDamage;

    [Inject]
    public void Construct(PlayerDetector playerDetector, CharacterStateMachine characterStateMachine)
    {
        this.playerDetector = playerDetector;
        this.characterStateMachine = characterStateMachine;
    }

    public void Initialize()
    {
        playerDetector.OnTakeDamage += HandleTakeDamage;
        playerDetector.OnCheckPoint += HandleCheckPoint;
    }

    public void Dispose()
    {
        playerDetector.OnTakeDamage -= HandleTakeDamage;
        playerDetector.OnCheckPoint -= HandleCheckPoint;
    }

    private void HandleTakeDamage(float damage)
    {
        (characterStateMachine as IPlayer)?.OnTakeDamage(damage);
    }
    private void HandleCheckPoint()
    {
        OnCheckPoint?.Invoke();
        (characterStateMachine as IPlayer)?.OnCheckPoint();
    }

    public Transform GetPlayerTransform() => characterStateMachine.GetTransform();

    public bool GetPlayerFallStatus() => (characterStateMachine as IPlayer).IsFalling;

    public CharacterStateMachine GetCharacterStateMachine => characterStateMachine;
    public void SetPlayerPosition(Vector3 pos) => (characterStateMachine as IPlayer).SetPosition(pos);
}
