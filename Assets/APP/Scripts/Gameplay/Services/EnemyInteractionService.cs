using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyInteractionService : IInitializable, IDisposable, ITickable
{
    private readonly PlayerInteractionService playerInteractionService;

    public bool IsCheckPointActive { get; private set; } = false;
    public Vector3 CharacterPosition { get; private set; } = Vector3.zero;

    [Inject]
    public EnemyInteractionService(PlayerInteractionService playerInteractionService)
    {
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
        if (!IsCheckPointActive) return;
        CharacterPosition = playerInteractionService.GetPlayerTransform().position;
    }

    private void HandleCharacterCheckPoint()
    {
        IsCheckPointActive = true;
    }
}