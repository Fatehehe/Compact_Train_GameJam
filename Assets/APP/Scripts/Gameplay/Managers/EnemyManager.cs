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

    // Variabel baru untuk delay spawn
    private LevelData currentLevelData;
    private bool isWaitingToSpawn = false;
    private float spawnDelayTimer = 0f;

    private bool canSpawn = true;

    [Inject]
    public EnemyManager(EnemyInteractionService enemyInteractionService, EnemySpawner enemySpawner)
    {
        this.enemyInteractionService = enemyInteractionService;
        this.enemySpawner = enemySpawner;
    }

    public void Initialize()
    {
        EnemyEvents.OnAttackAnimationCompleted += HandleAttackAnimationCompleted;
    }

    public void Dispose()
    {
        EnemyEvents.OnAttackAnimationCompleted -= HandleAttackAnimationCompleted; // Perbaikan: Seharusnya -= saat Dispose
    }

    public void SetupLevel(LevelData levelData)
    {
        ClearAllEnemies();
        currentLevelData = levelData; // Simpan referensi level data
        spawnCounter = levelData.checkpointEnemyCount;
        isWaitingToSpawn = false; // Reset status spawn
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

    private void HandleAttackAnimationCompleted()
    {
        if (currentEnemy is MonoBehaviour enemyComponent)
        {
            UnityEngine.Object.Destroy(enemyComponent.gameObject);
        }

        currentEnemy = null;
    }

    public void Tick()
    {
        if (!canSpawn || !enemyInteractionService.IsCheckPointActive) return;

        bool isEnemyDead = currentEnemy == null || currentEnemy.Equals(null) || currentEnemy.IsKnockedOut;

        if (isEnemyDead)
        {
            if (enemyInteractionService.IsSpawnReady) return;

            if (spawnCounter > 0)
            {
                // Jika belum menunggu spawn, mulai timer
                if (!isWaitingToSpawn)
                {
                    isWaitingToSpawn = true;
                    // Ambil waktu acak berdasarkan min dan max dari level data
                    // Gunakan UnityEngine.Random untuk menghindari bentrok dengan System.Random
                    spawnDelayTimer = UnityEngine.Random.Range(currentLevelData.randomMinTimeSpawn, currentLevelData.randomMaxTimeSpawn);
                }
                else
                {
                    // Hitung mundur timer
                    spawnDelayTimer -= Time.deltaTime;

                    if (spawnDelayTimer <= 0)
                    {
                        // Waktu habis, saatnya spawn
                        spawnCounter--;
                        isWaitingToSpawn = false;
                        SpawnAtRandomPosition();
                    }
                }
            }
            return;
        }

        if (currentEnemy is MonoBehaviour enemyComp)
        {
            float distanceSqr = (enemyInteractionService.CharacterPosition - enemyComp.transform.position).sqrMagnitude;
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

    // Metode baru untuk random posisi spawn
    private void SpawnAtRandomPosition()
    {
        // Masukkan semua titik spawn ke dalam array
        Transform[] spawnPoints = new Transform[]
        {
            enemySpawner.RightSpawnPosition,
            enemySpawner.LeftSpawnPosition,
            enemySpawner.BehindSpawnPosition
        };

        // Pilih indeks acak dari 0 sampai panjang array (3)
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        // Pastikan titik spawn tidak null sebelum spawn
        if (selectedSpawnPoint != null)
        {
            Spawn(enemySpawner.Prefab, selectedSpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Titik spawn belum diatur di EnemySpawner!");
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

    public void StopSpawning()
    {
        canSpawn = false; // Matikan izin spawn
        ClearAllEnemies(); // Hapus musuh yang sedang ada di map (opsional)
    }
}