using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyManager : IInitializable, IDisposable, ITickable
{
    private readonly EnemyInteractionService enemyInteractionService;
    private readonly EnemySpawner enemySpawner;
    private readonly IObjectResolver resolver;

    private IEnemy currentEnemy; // Hanya simpan 1 musuh aktif
    private int spawnCounter = 2; // Total musuh yang akan dispawn bergantian
    private readonly float catchDistanceSqr = 0.5f * 0.5f; // Jarak tangkap (dikuadratkan)

    [Inject] // Gunakan Constructor Injection agar rapi
    public EnemyManager(EnemyInteractionService enemyInteractionService, EnemySpawner enemySpawner, IObjectResolver resolver)
    {
        this.enemyInteractionService = enemyInteractionService;
        this.enemySpawner = enemySpawner;
        this.resolver = resolver;
    }

    public void Initialize() { }
    public void Dispose() { }

    public void Tick()
    {
        // 1. Jangan ngapa-ngapain kalau player belum sentuh checkpoint
        if (!enemyInteractionService.IsCheckPointActive) return;

        // 2. Cek status musuh saat ini (Apakah kosong? hancur? atau sudah KO?)
        bool isEnemyDead = (currentEnemy == null || currentEnemy.Equals(null) || currentEnemy.IsKnockedOut);

        if (isEnemyDead)
        {
            // Jika musuh mati, dan masih ada jatah spawn, maka spawn lagi!
            if (spawnCounter > 0)
            {
                spawnCounter--;
                Spawn(enemySpawner.Prefab, enemySpawner.RightSpawnPosition.position);
            }
            return; // Tunggu frame selanjutnya agar musuh sempat terinisialisasi
        }

        // 3. Jika musuh masih hidup, lakukan logika kejar
        if (currentEnemy is MonoBehaviour enemyComponent)
        {
            // Hitung jarak ke player
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
        if (prefab == null) return;

        // Tetap wajib pakai resolver.Instantiate agar VContainer jalan di dalam musuh
        GameObject newEnemyObj = resolver.Instantiate(prefab, position, Quaternion.identity);

        if (newEnemyObj.TryGetComponent(out IEnemy enemyInterface))
        {
            currentEnemy = enemyInterface;
        }
    }
}