using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class EnemyAnimationService : IInitializable, IDisposable
{

    [Inject]
    public void Construct(PlayerInteractionService playerInteractionService)
    {
        // this.playerInteractionService = playerInteractionService;
    }

    public void Initialize()
    {
        // playerInteractionService.OnCheckPoint += HandleCharacterCheckPoint;
    }

    public void Dispose()
    {
        // playerInteractionService.OnCheckPoint -= HandleCharacterCheckPoint;
    }

    public void AnimateChasing(List<IEnemy> enemies, Vector3 position)
    {
        foreach (IEnemy enemy in enemies)
        {
            // enemy.OnTargetDetected(position);
        }
    }
}
