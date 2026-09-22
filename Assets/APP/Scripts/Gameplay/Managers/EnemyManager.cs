using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyManager : IInitializable, IDisposable, ITickable
{
    private readonly EnemyInteractionService enemyInteractionService;
    private readonly EnemySpawner enemySpawner;
    private IEnemy currentEnemy;
    private int spawnCounter = 2;
    private readonly float catchDistanceSqr = 0.5f * 0.5f;

    [Inject]
    public EnemyManager(EnemyInteractionService enemyInteractionService, EnemySpawner enemySpawner)
    {
        this.enemyInteractionService = enemyInteractionService;
        this.enemySpawner = enemySpawner;
    }

    public void Initialize()
    {
        EnemyEvents.OnAttackCompleted += HandleAttackCompleted;
        EnemyEvents.OnReturnCompleted += HandleReturnCompleted;
    }
    public void Dispose()
    {
        EnemyEvents.OnAttackCompleted -= HandleAttackCompleted;
        EnemyEvents.OnReturnCompleted -= HandleReturnCompleted;
    }

    private void HandleReturnCompleted()
    {
        if (currentEnemy is MonoBehaviour enemyComponent)
        {
            UnityEngine.Object.Destroy(enemyComponent.gameObject);
        }

        currentEnemy = null;
    }

    private void HandleAttackCompleted()
    {
        currentEnemy?.OnReturn(enemySpawner.LeftSpawnPosition.position);
    }

    public void Tick()
    {
        Debug.Log("fall status " + enemyInteractionService.IsSpawnReady);
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