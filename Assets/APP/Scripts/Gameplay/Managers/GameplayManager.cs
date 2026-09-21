using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayManager : IInitializable, IDisposable
{
    private EnemyManager enemyManager;
    private PlayerManager playerManager;
    private PlayerDetector playerDetector;

    [Inject]
    public void Construct(PlayerManager playerManager, EnemyManager enemyManager, PlayerDetector playerDetector)
    {
        this.playerManager = playerManager;
        this.enemyManager = enemyManager;
        this.playerDetector = playerDetector;
    }

    public void Initialize()
    {
        playerDetector.OnFinishReached += HandleFinishReached;
    }

    public void Dispose()
    {
        playerDetector.OnFinishReached -= HandleFinishReached;
    }

    private void HandleFinishReached()
    {
        playerManager.StopCharacter();
        // enemyManager.StopAllEnemies();
    }

    private void NextLevel()
    {

    }
}
