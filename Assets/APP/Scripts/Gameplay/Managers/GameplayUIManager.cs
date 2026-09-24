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

        uiEndgameController.OnHomeButtonPressed += HandleHomeButtonPressed;
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
        uiEndgameController.OnHomeButtonPressed -= HandleHomeButtonPressed;
        uiEndgameController.OnNextButtonPressed -= HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed -= HandleRestartButtonPressed;

        gameplayManager.OnGameEnded -= HandleGameEnded;
        gameplayManager.OnGameStarted -= HandleGameRunning;
    }

    private void HandleGameStart()
    {
        if (gameplayManager.IsGameRunning) return;
        gameplayManager.StartGame();
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
        uiEndgameController.SetResultText(isWinning);
    }

    private void HandleHomeButtonPressed()
    {
        uiEndgameController.SetActive(false);
        uiMainController.SetActive(true);
    }

    private void HandleNextButtonPressed()
    {
        gameplayManager.NextLevel();
    }

    private void HandleRestartButtonPressed()
    {
        gameplayManager.RestartGame();
    }
}