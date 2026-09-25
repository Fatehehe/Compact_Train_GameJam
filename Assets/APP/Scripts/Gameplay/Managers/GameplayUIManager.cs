using System;
using UnityEngine;
using VContainer;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private UIMainController uiMainController;
    [SerializeField] private UIGameplayController uiGameplayController;
    [SerializeField] private UIEndgameController uiEndgameController;
    [SerializeField] private UITutorialController uiTutorialController;
    [SerializeField] private GameObject enviHome;

    private GameplayManager gameplayManager;

    [Inject]
    public void Construct(GameplayManager gameplayManager, IObjectResolver container)
    {
        this.gameplayManager = gameplayManager;

        container.Inject(uiMainController);
        container.Inject(uiGameplayController);
        container.Inject(uiEndgameController);
        container.Inject(uiTutorialController);
    }

    private void Awake()
    {
        uiMainController.OnGameStart += HandleGameStart;

        uiEndgameController.OnNextButtonPressed += HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed += HandleRestartButtonPressed;

        uiTutorialController.OnTutorialFinished += HandleTutorialFinished;

        gameplayManager.OnGameEnded += HandleGameEnded;
        gameplayManager.OnGameStarted += HandleGameRunning;

        // [TAMBAHKAN INI] Dengarkan saat semua level habis
        gameplayManager.OnAllLevelsFinished += HandleAllLevelsFinished;

        // Kondisi awal (Game baru dibuka)
        uiMainController.SetActive(true);
        uiGameplayController.SetActive(false);
        uiEndgameController.SetActive(false);
        uiTutorialController.SetActive(false);

        if (enviHome != null) enviHome.SetActive(true); // Pastikan enviHome muncul di awal
    }

    void OnDestroy()
    {
        uiMainController.OnGameStart -= HandleGameStart;
        uiEndgameController.OnNextButtonPressed -= HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed -= HandleRestartButtonPressed;

        uiTutorialController.OnTutorialFinished -= HandleTutorialFinished;

        gameplayManager.OnGameEnded -= HandleGameEnded;
        gameplayManager.OnGameStarted -= HandleGameRunning;

        // [TAMBAHKAN INI]
        gameplayManager.OnAllLevelsFinished -= HandleAllLevelsFinished;
    }

    private void HandleGameStart()
    {
        if (gameplayManager.IsGameRunning) return;

        if (gameplayManager.CurrentLevelIndex == 0)
        {
            uiMainController.SetActive(false);
            uiTutorialController.ShowTutorial();
        }
        else
        {
            StartGameplayProcess();
        }
    }

    private void HandleTutorialFinished()
    {
        StartGameplayProcess();
    }

    private void StartGameplayProcess()
    {
        gameplayManager.StartGame();
        uiGameplayController.SetLevelText(gameplayManager.GetLevelIndex());
        uiGameplayController.ResetAllIndicators();
    }

    private void HandleGameRunning()
    {
        // [TAMBAHKAN INI] Sembunyikan enviHome saat mulai main
        if (enviHome != null) enviHome.SetActive(false);

        uiGameplayController.SetLevelText(gameplayManager.GetLevelIndex());
        uiMainController.SetActive(false);
        uiEndgameController.SetActive(false);
        uiTutorialController.SetActive(false);
        uiGameplayController.SetActive(true);
        uiGameplayController.ResetAllIndicators();
    }

    private void HandleGameEnded(bool isWinning)
    {
        uiGameplayController.SetActive(false);
        uiEndgameController.SetActive(true);
        uiGameplayController.ResetAllIndicators();
        uiEndgameController.SetResultText(isWinning, gameplayManager.GetLevelIndex());
    }

    private void HandleNextButtonPressed()
    {
        uiGameplayController.ResetAllIndicators();
        gameplayManager.NextLevel();
    }

    private void HandleRestartButtonPressed()
    {
        uiGameplayController.ResetAllIndicators();
        gameplayManager.RestartGame();
    }

    // [TAMBAHKAN FUNGSI INI] Dipanggil saat menang dan klik next tapi level habis
    private void HandleAllLevelsFinished()
    {
        if (enviHome != null) enviHome.SetActive(true); // Munculkan kembali enviHome

        // Kembalikan ke menu utama
        uiEndgameController.SetActive(false);
        uiGameplayController.SetActive(false);
        uiTutorialController.SetActive(false);
        uiMainController.SetActive(true);
    }
}