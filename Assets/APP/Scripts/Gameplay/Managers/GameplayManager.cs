using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayManager : IInitializable, IDisposable, ITickable
{
    private EnemyManager enemyManager;
    private PlayerManager playerManager;
    private PlayerDetector playerDetector;
    private LevelDatabase levelDatabase;

    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;
    private float timer;
    private bool isGameRunning = false;

    [Inject]
    public void Construct(PlayerManager playerManager, EnemyManager enemyManager, PlayerDetector playerDetector, LevelDatabase levelDatabase)
    {
        this.playerManager = playerManager;
        this.enemyManager = enemyManager;
        this.playerDetector = playerDetector;
        this.levelDatabase = levelDatabase;
    }

    public void Initialize()
    {
        playerDetector.OnFinishReached += HandleFinishReached;
        StartLevel(currentLevelIndex);
    }

    public void Dispose()
    {
        playerDetector.OnFinishReached -= HandleFinishReached;
    }

    private void StartLevel(int index)
    {
        LevelData levelData = levelDatabase.GetLevel(index);

        if (levelData == null)
        {
            Debug.Log("Semua Level Selesai! Game Tamat.");
            return;
        }

        LevelEnvironmentData envData = levelData.levelEnvironment;

        if (currentLevelInstance != null)
        {
            UnityEngine.Object.Destroy(currentLevelInstance);
        }

        currentLevelInstance = UnityEngine.Object.Instantiate(
            envData.environmentPrefab,
            envData.environmentSpawnPosition,
            Quaternion.identity
        );

        playerManager.SetPlayerPosition(envData.playerStartPosition);
        playerManager.ResetAnimation();
        enemyManager.SetupLevel(levelData);

        timer = levelData.levelTimer;
        isGameRunning = true;

        Debug.Log($"Level {index + 1} Dimulai! Waktu: {timer} detik");
    }

    public void Tick()
    {
        if (!isGameRunning) return;
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        isGameRunning = false;
        playerManager.StopCharacter(false);
        enemyManager.StopSpawning();
    }

    private void HandleFinishReached()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        playerManager.StopCharacter(true);
        enemyManager.StopSpawning();
    }

    private void NextLevel()
    {
        currentLevelIndex++;
        StartLevel(currentLevelIndex);
    }

    private void RestartGame()
    {
        currentLevelIndex = 0;
        StartLevel(0);
    }
}