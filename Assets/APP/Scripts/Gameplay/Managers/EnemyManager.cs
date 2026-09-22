using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyManager : IInitializable, IDisposable, ITickable
{
    private readonly EnemyInteractionService enemyInteractionService;
    private readonly EnemySpawner enemySpawner;
    private IEnemy currentEnemy;

    private int spawnCounter;
    private readonly float catchDistanceSqr = 0.8f * 0.8f;

    private List<IEnemy> activePushEnemies = new();

    [Inject]
    public EnemyManager(EnemyInteractionService enemyInteractionService, EnemySpawner enemySpawner)
    {
        this.enemyInteractionService = enemyInteractionService;
        this.enemySpawner = enemySpawner;
    }

    public void Initialize()
    {
        EnemyEvents.OnAttackCompleted += HandleAttackCompleted;
        EnemyEvents.OnAttackAnimationCompleted += HandleAttackAnimationCompleted;
        EnemyEvents.OnReturnCompleted += HandleReturnCompleted;
    }

    public void Dispose()
    {
        EnemyEvents.OnAttackCompleted -= HandleAttackCompleted;
        EnemyEvents.OnAttackAnimationCompleted += HandleAttackAnimationCompleted;
        EnemyEvents.OnReturnCompleted -= HandleReturnCompleted;
    }

    public void SetupLevel(LevelData levelData)
    {
        ClearAllEnemies();
        spawnCounter = levelData.checkpointEnemyCount;

        if (levelData.pushEnemyPrefab != null && levelData.levelEnvironment.pushEnemyPositions != null)
        {
            foreach (Vector3 pos in levelData.levelEnvironment.pushEnemyPositions)
            {
                GameObject pushEnemy = UnityEngine.Object.Instantiate(levelData.pushEnemyPrefab, pos, Quaternion.identity);
                pushEnemy.TryGetComponent(out IEnemy enemy);
                activePushEnemies.Add(enemy);
                // enemy.OnPushPerformed();
            }
        }
    }

    public void ClearAllEnemies()
    {
        if (currentEnemy is MonoBehaviour enemyComponent)
        {
            UnityEngine.Object.Destroy(enemyComponent.gameObject);
        }
        currentEnemy = null;

        foreach (IEnemy obj in activePushEnemies)
        {
            if (obj is Component gameobj)
            {
                UnityEngine.Object.Destroy(gameobj.gameObject);
            }
        }
        activePushEnemies.Clear();
    }

    private void HandleReturnCompleted()
    {
        if (currentEnemy is MonoBehaviour enemyComponent)
        {
            UnityEngine.Object.Destroy(enemyComponent.gameObject);
        }

        currentEnemy = null;
    }

    private void HandleAttackAnimationCompleted()
    {
        currentEnemy?.OnReturn(enemySpawner.LeftSpawnPosition.position);
    }

    private void HandleAttackCompleted()
    {
        // currentEnemy?.OnReturn(enemySpawner.LeftSpawnPosition.position);
    }

    public void Tick()
    {
        if (!enemyInteractionService.IsCheckPointActive) return;

        bool isEnemyDead = currentEnemy == null || currentEnemy.Equals(null) || currentEnemy.IsKnockedOut;

        if (isEnemyDead)
        {
            if (enemyInteractionService.IsSpawnReady) return;
            if (spawnCounter > 0)
            {
                spawnCounter--;
                Spawn(enemySpawner.Prefab, enemySpawner.RightSpawnPosition.position);
            }
            return;
        }

        if (currentEnemy is MonoBehaviour enemyComponent)
        {
            float distanceSqr = (enemyInteractionService.CharacterPosition - enemyComponent.transform.position).sqrMagnitude;
            if (distanceSqr <= catchDistanceSqr)
            {
                currentEnemy.OnTargetReached();
            }
            else
            {
                currentEnemy.OnChasingPerformed(enemyInteractionService.CharacterPosition);
            }
        }
    }

    private void Spawn(GameObject prefab, Vector3 position)
    {
        GameObject obj = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        if (obj.TryGetComponent(out IEnemy enemyInterface))
        {
            currentEnemy = enemyInterface;
        }
    }
}