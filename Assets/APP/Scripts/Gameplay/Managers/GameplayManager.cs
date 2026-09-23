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

    public bool IsGameRunning => isGameRunning;
    private bool isGameRunning = false;

    public event Action<bool> OnGameEnded;
    public event Action OnGameStarted;

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
        GameEvents.OnMissHit += HandleMissHit;
        GameEvents.OnPlay += Play;
    }

    public void Dispose()
    {
        playerDetector.OnFinishReached -= HandleFinishReached;
        GameEvents.OnMissHit -= HandleMissHit;
        GameEvents.OnPlay -= Play;
    }

    private void HandleMissHit(float penaltyTime)
    {
        if (!isGameRunning) return;

        timer -= penaltyTime;
        GameEvents.OnTimerUpdated?.Invoke(timer);

        Debug.Log($"MISS! Waktu dikurangi {penaltyTime} detik. Sisa waktu: {timer}");

        if (timer <= 0)
        {
            timer = 0;
            HandleGameOver();
        }
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
        playerManager.ResetPlayer();
        enemyManager.SetupLevel(levelData);

        timer = levelData.levelTimer;
        GameEvents.OnTimerUpdated?.Invoke(timer);

        Debug.Log($"Level {index + 1} Dimulai! Waktu: {timer} detik");
        OnGameStarted.Invoke();
    }

    public void Tick()
    {
        if (!isGameRunning) return;
        timer -= Time.deltaTime;

        GameEvents.OnTimerUpdated?.Invoke(timer);

        if (timer <= 0)
        {
            timer = 0;
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        isGameRunning = false;
        OnGameEnded.Invoke(false);
        playerManager.StopCharacter(false);
        enemyManager.StopSpawning();
    }

    private void HandleFinishReached()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        OnGameEnded.Invoke(true);
        playerManager.StopCharacter(true);
        enemyManager.StopSpawning();
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        StartLevel(currentLevelIndex);
    }

    public void StartGame()
    {
        currentLevelIndex = 0;
        StartLevel(0);
    }

    public void RestartGame()
    {
        StartLevel(currentLevelIndex);
    }

    public void Play()
    {
        isGameRunning = true;
    }
}