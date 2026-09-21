using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyManager : IInitializable, IDisposable
{
    private EnemyInteractionService enemyInteractionService;
    private EnemySpawner enemySpawner;

    private bool isChasingStart = false;
    private int spawnCounter = 2;

    [Inject]
    public void Construct(EnemyInteractionService enemyInteractionService, EnemySpawner enemySpawner)
    {
        this.enemyInteractionService = enemyInteractionService;
        this.enemySpawner = enemySpawner;
    }

    public void Initialize()
    {
        enemyInteractionService.OnEnemyChasingStarted += HandleEnemyChasingStarted;
        enemyInteractionService.OnEnemyDetectingCharacter += HandleEnemyDetectingCharacter;
    }

    public void Dispose()
    {
        enemyInteractionService.OnEnemyChasingStarted -= HandleEnemyChasingStarted;
        enemyInteractionService.OnEnemyDetectingCharacter -= HandleEnemyDetectingCharacter;
    }

    private void HandleEnemyChasingStarted(Vector3 vector)
    {
        isChasingStart = true;
    }

    private void HandleEnemyDetectingCharacter(Vector3 vector)
    {
        if (!isChasingStart) return;

        if (spawnCounter == 1)
        {
            spawnCounter--;
            Spawn(enemySpawner.Prefab, enemySpawner.LeftSpawnPosition.position);
        }
        else if (spawnCounter == 2)
        {
            spawnCounter--;
            Spawn(enemySpawner.Prefab, enemySpawner.RightSpawnPosition.position);
        }
    }

    private void Spawn(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return;

        Quaternion rot = Quaternion.Euler(Vector3.zero);
        UnityEngine.Object.Instantiate(prefab, position, rot);
    }
}