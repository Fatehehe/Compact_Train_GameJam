using System;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

public class PlayerManager : IInitializable, IDisposable
{
    private PlayerInteractionService playerInteractionService;
    private PlayerAnimationService playerAnimationService;

    [Inject]
    public void Construct(PlayerInteractionService playerInteractionService, PlayerAnimationService playerAnimationService)
    {
        this.playerAnimationService = playerAnimationService;
        this.playerInteractionService = playerInteractionService;
    }

    public void Initialize()
    {
    }

    public void Dispose()
    {
    }

    public void StopCharacter(bool isWin)
    {
        if (isWin)
        {
            playerAnimationService.WinCharacterAnimation();
        }
        else
        {
            playerAnimationService.LoseCharacterAnimation();
        }
    }

    public void ResetPlayer()
    {
        playerInteractionService.GetCharacterStateMachine.SwitchState(new CharacterIdleState(playerInteractionService.GetCharacterStateMachine));
    }

    public void SetPlayerPosition(Vector3 startPosition)
    {
        playerInteractionService.SetPlayerPosition(startPosition);
    }


}