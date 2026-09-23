using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyManager : IInitializable, IDisposable, ITickable
{
    private readonly EnemyInteractionService enemyInteractionService;
    private readonly PlayerInteractionService playerInteractionService;
    private readonly HapticManager hapticManager;

    private readonly EnemySpawner enemySpawner;
    private IEnemy currentEnemy;

    private readonly List<IEnemy> activePushEnemies = new();

    private LevelData currentLevelData;
    private bool isWaitingToSpawn = false;
    private float spawnDelayTimer = 0f;

    private bool canSpawn = true;

    // Pastikan catch distance selalu lebih kecil dari minHitDistance agar musuh tidak nabrak sebelum sweet spot habis
    private readonly float catchDistanceSqr = 0.4f * 0.4f;

    [Inject]
    public EnemyManager(PlayerInteractionService playerInteractionService, EnemyInteractionService enemyInteractionService, EnemySpawner enemySpawner, HapticManager hapticManager)
    {
        this.playerInteractionService = playerInteractionService;
        this.enemyInteractionService = enemyInteractionService;
        this.enemySpawner = enemySpawner;
        this.hapticManager = hapticManager;
    }

    public void Initialize()
    {
        // PASTIKAN pakai parameter GameObject di script EnemyEvents
        EnemyEvents.OnAnimationCompleted += HandleAnimationCompleted;
        GameEvents.OnPlayerSwipe += HandlePlayerAttackCheck;
    }

    public void Dispose()
    {
        EnemyEvents.OnAnimationCompleted -= HandleAnimationCompleted;
        GameEvents.OnPlayerSwipe -= HandlePlayerAttackCheck;
    }

    public void SetupLevel(LevelData levelData)
    {
        ClearAllEnemies();
        enemyInteractionService.ResetStatus();
        GameEvents.OnCheckpointStateChanged?.Invoke(false);

        currentLevelData = levelData;
        isWaitingToSpawn = false;
        canSpawn = true;

        if (levelData.pushEnemyPrefab != null && levelData.levelEnvironment.pushEnemyPositions != null)
        {
            foreach (Vector3 pos in levelData.levelEnvironment.pushEnemyPositions)
            {
                GameObject pushEnemy = UnityEngine.Object.Instantiate(levelData.pushEnemyPrefab, pos, Quaternion.identity);
                pushEnemy.TryGetComponent(out IEnemy enemy);
                activePushEnemies.Add(enemy);
            }
        }
    }

    private void HandlePlayerAttackCheck(string swipeDirection)
    {
        if (!canSpawn || currentLevelData == null) return;

        bool isMiss = true;

        if (currentEnemy != null && currentEnemy is MonoBehaviour enemyComp && !currentEnemy.IsKnockedOut)
        {
            float distance = Vector3.Distance(enemyInteractionService.CharacterPosition, enemyComp.transform.position);
            bool isInSweetSpot = distance >= currentLevelData.minHitDistance && distance <= currentLevelData.maxHitDistance;

            Vector3 dirToEnemy = (enemyComp.transform.position - enemyInteractionService.CharacterPosition).normalized;
            bool isDirectionCorrect = false;

            if (swipeDirection == "Right" && dirToEnemy.x > 0.3f) isDirectionCorrect = true;
            else if (swipeDirection == "Left" && dirToEnemy.x < -0.3f) isDirectionCorrect = true;
            else if (swipeDirection == "Down" && dirToEnemy.z < -0.3f) isDirectionCorrect = true;

            if (isInSweetSpot && isDirectionCorrect)
            {
                currentEnemy.OnKnockedOut();
                isMiss = false;
                Debug.Log("NICE HIT! Musuh dikalahkan.");

                // LANGSUNG HIDE UI saat musuh berhasil dipukul
                hapticManager.Heavy();
                GameEvents.OnEnemyClear?.Invoke();
            }
        }

        if (isMiss)
        {
            GameEvents.OnMissHit?.Invoke(currentLevelData.timePenalty);
        }
    }

    public void Tick()
    {
        if (!canSpawn || !enemyInteractionService.IsCheckPointActive) return;
        bool isEnemyDead = currentEnemy == null || currentEnemy.Equals(null);

        if (isEnemyDead)
        {
            if (enemyInteractionService.IsSpawnReady) return;
            if (!isWaitingToSpawn)
            {
                isWaitingToSpawn = true;
                spawnDelayTimer = UnityEngine.Random.Range(currentLevelData.randomMinTimeSpawn, currentLevelData.randomMaxTimeSpawn);
            }
            else
            {
                spawnDelayTimer -= Time.deltaTime;
                if (spawnDelayTimer <= 0)
                {
                    isWaitingToSpawn = false;
                    SpawnAtRandomPosition();
                }
            }
            return;
        }

        if (currentEnemy.IsKnockedOut) return;

        if (currentEnemy is MonoBehaviour enemyComp)
        {
            float distance = Vector3.Distance(enemyInteractionService.CharacterPosition, enemyComp.transform.position);

            Vector3 dirToEnemy = (enemyComp.transform.position - enemyInteractionService.CharacterPosition).normalized;
            string targetDir = "None";
            if (dirToEnemy.x > 0.3f) targetDir = "Right";
            else if (dirToEnemy.x < -0.3f) targetDir = "Left";
            else if (dirToEnemy.z < -0.3f) targetDir = "Down";

            float startTrackDistance = 10f;

            // Jika musuh masih di luar radar, ATAU musuh sudah kelewat batas sweet spot (terlalu dekat)
            if (distance > startTrackDistance || distance < currentLevelData.minHitDistance)
            {
                // Sembunyikan UI
                GameEvents.OnEnemyClear?.Invoke();
            }
            else
            {
                // Hitung progres Fill Bar (dari 0 ke 1)
                float progress = Mathf.InverseLerp(startTrackDistance, currentLevelData.maxHitDistance, distance);
                bool inSweetSpot = distance <= currentLevelData.maxHitDistance;

                // Jika sudah masuk sweet spot, pastikan bar-nya penuh 100%
                if (inSweetSpot) progress = 1f;

                GameEvents.OnEnemyApproachUpdate?.Invoke(targetDir, progress, inSweetSpot, false);
            }

            // Logika Tabrakan (Jarak ini harus diset lebih kecil dari minHitDistance di LevelData)
            float distanceSqr = distance * distance;
            if (distanceSqr <= catchDistanceSqr)
            {
                currentEnemy.OnTargetReached();
                playerInteractionService.GetCharacterStateMachine.OnKnockedOut();
                GameEvents.OnEnemyClear?.Invoke(); // Pastikan indikator hilang jika menabrak
            }
            else
            {
                currentEnemy.OnChasingPerformed(enemyInteractionService.CharacterPosition);
            }
        }
    }

    private void SpawnAtRandomPosition()
    {
        Transform[] spawnPoints = new Transform[]
        {
            enemySpawner.RightSpawnPosition,
            enemySpawner.LeftSpawnPosition,
            enemySpawner.BehindSpawnPosition
        };

        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        if (selectedSpawnPoint != null) Spawn(enemySpawner.Prefab, selectedSpawnPoint.position);
    }

    private void Spawn(GameObject prefab, Vector3 position)
    {
        GameObject obj = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        if (obj.TryGetComponent(out IEnemy enemyInterface))
        {
            currentEnemy = enemyInterface;
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;
        ClearAllEnemies();
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
            if (obj is Component gameobj) UnityEngine.Object.Destroy(gameobj.gameObject);
        }

        activePushEnemies.Clear();
    }

    private void HandleAnimationCompleted()
    {
        if (currentEnemy is MonoBehaviour enemyComponent)
            UnityEngine.Object.Destroy(enemyComponent.gameObject);
        currentEnemy = null;
    }
}