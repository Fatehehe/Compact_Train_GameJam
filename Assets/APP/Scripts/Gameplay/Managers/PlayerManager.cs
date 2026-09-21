using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerManager : IInitializable, IDisposable
{

    private PlayerInteractionService playerInteractionService;
    private PlayerAnimationService playerAnimationService;
    private PlayerControlService playerControlService;

    [Inject]
    public void Construct(PlayerInteractionService playerInteractionService, PlayerAnimationService playerAnimationService, PlayerControlService playerControlService)
    {
        this.playerAnimationService = playerAnimationService;
        this.playerInteractionService = playerInteractionService;
        this.playerControlService = playerControlService;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    public void StopCharacter()
    {
        playerAnimationService.StopCharacterAnimation();
    }
}
