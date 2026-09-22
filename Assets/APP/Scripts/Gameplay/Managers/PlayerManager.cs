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

    public void ResetAnimation()
    {
        playerInteractionService.GetCharacterStateMachine.SwitchState(new CharacterIdleState(playerInteractionService.GetCharacterStateMachine));
    }

    // FUNGSI BARU: Mengatur posisi player di awal level
    public void SetPlayerPosition(Vector3 startPosition)
    {
        // CATATAN: Ganti "CharacterGameObject" dengan properti asli yang menyimpan Transform/GameObject player di dalam servicemu.
        // Contoh:
        // playerInteractionService.CharacterTransform.position = startPosition;
    }
}