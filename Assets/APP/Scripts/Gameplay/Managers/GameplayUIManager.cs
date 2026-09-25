using System;
using UnityEngine;
using VContainer;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private UIMainController uiMainController;
    [SerializeField] private UIGameplayController uiGameplayController;
    [SerializeField] private UIEndgameController uiEndgameController;
    [SerializeField] private UITutorialController uiTutorialController; // [TAMBAHKAN INI]

    private GameplayManager gameplayManager;

    [Inject]
    public void Construct(GameplayManager gameplayManager, IObjectResolver container)
    {
        this.gameplayManager = gameplayManager;

        container.Inject(uiMainController);
        container.Inject(uiGameplayController);
        container.Inject(uiEndgameController);
        container.Inject(uiTutorialController); // [TAMBAHKAN INI]
    }

    private void Awake()
    {
        uiMainController.OnGameStart += HandleGameStart;

        uiEndgameController.OnNextButtonPressed += HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed += HandleRestartButtonPressed;

        uiTutorialController.OnTutorialFinished += HandleTutorialFinished; // [TAMBAHKAN INI]

        gameplayManager.OnGameEnded += HandleGameEnded;
        gameplayManager.OnGameStarted += HandleGameRunning;

        uiMainController.SetActive(true);
        uiGameplayController.SetActive(false);
        uiEndgameController.SetActive(false);
        uiTutorialController.SetActive(false); // Pastikan tutorial disembunyikan di awal
    }

    void OnDestroy()
    {
        uiMainController.OnGameStart -= HandleGameStart;
        uiEndgameController.OnNextButtonPressed -= HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed -= HandleRestartButtonPressed;

        uiTutorialController.OnTutorialFinished -= HandleTutorialFinished; // [TAMBAHKAN INI]

        gameplayManager.OnGameEnded -= HandleGameEnded;
        gameplayManager.OnGameStarted -= HandleGameRunning;
    }

    private void HandleGameStart()
    {
        if (gameplayManager.IsGameRunning) return;

        // Cek jika ini adalah Level Pertama (Index 0)
        if (gameplayManager.CurrentLevelIndex == 0)
        {
            uiMainController.SetActive(false);
            uiTutorialController.ShowTutorial(); // Tampilkan Tutorial
        }
        else
        {
            StartGameplayProcess(); // Langsung main
        }
    }

    private void HandleTutorialFinished()
    {
        StartGameplayProcess();
    }

    // Fungsi helper agar tidak mengulang penulisan kode
    private void StartGameplayProcess()
    {
        gameplayManager.StartGame();
        uiGameplayController.SetLevelText(gameplayManager.GetLevelIndex());
        uiGameplayController.ResetAllIndicators();
    }

    private void HandleGameRunning()
    {
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
}