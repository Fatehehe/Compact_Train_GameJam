using System;
using UnityEngine;
using VContainer;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private UIMainController uiMainController;
    [SerializeField] private UIGameplayController uiGameplayController;
    [SerializeField] private UIEndgameController uiEndgameController;

    private GameplayManager gameplayManager;

    [Inject]
    public void Construct(GameplayManager gameplayManager, IObjectResolver container)
    {
        this.gameplayManager = gameplayManager;

        container.Inject(uiMainController);
        container.Inject(uiGameplayController);
        container.Inject(uiEndgameController);
    }

    private void Awake()
    {
        uiMainController.OnGameStart += HandleGameStart;

        uiEndgameController.OnNextButtonPressed += HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed += HandleRestartButtonPressed;

        gameplayManager.OnGameEnded += HandleGameEnded;
        gameplayManager.OnGameStarted += HandleGameRunning;

        uiMainController.SetActive(true);
        uiGameplayController.SetActive(false);
        uiEndgameController.SetActive(false);
    }

    void OnDestroy()
    {
        uiMainController.OnGameStart -= HandleGameStart;
        uiEndgameController.OnNextButtonPressed -= HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed -= HandleRestartButtonPressed;

        gameplayManager.OnGameEnded -= HandleGameEnded;
        gameplayManager.OnGameStarted -= HandleGameRunning;
    }

    private void HandleGameStart()
    {
        if (gameplayManager.IsGameRunning) return;
        gameplayManager.StartGame();
        uiGameplayController.SetLevelText(gameplayManager.GetLevelIndex());
        uiGameplayController.ResetAllIndicators();
    }

    private void HandleGameRunning()
    {
        uiMainController.SetActive(false);
        uiEndgameController.SetActive(false);
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