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
        container.Inject(uiEndgameController); // Tambahkan ini agar aman jika UIEndgame butuh DI
    }

    private void Awake()
    {
        // Bind tombol-tombol UI
        uiMainController.OnGameStart += HandleGameStart;
        uiEndgameController.OnHomeButtonPressed += HandleHomeButtonPressed;
        uiEndgameController.OnNextButtonPressed += HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed += HandleRestartButtonPressed;

        // Bind event dari GameplayManager
        gameplayManager.OnGameEnded += HandleGameEnded;
        gameplayManager.OnGameStarted += HandleGameRunning; // <-- PERBAIKAN: Gunakan HandleGameRunning, bukan HandleGameStart

        // Setup awal saat game baru dibuka (Main Menu nyala, sisanya mati)
        uiMainController.SetActive(true);
        uiGameplayController.SetActive(false);
        uiEndgameController.SetActive(false);
    }

    void OnDestroy()
    {
        // Unbind event UI
        uiMainController.OnGameStart -= HandleGameStart;
        uiEndgameController.OnHomeButtonPressed -= HandleHomeButtonPressed;
        uiEndgameController.OnNextButtonPressed -= HandleNextButtonPressed;
        uiEndgameController.OnRestartButtonPressed -= HandleRestartButtonPressed;

        // Unbind event GameplayManager
        gameplayManager.OnGameEnded -= HandleGameEnded;
        gameplayManager.OnGameStarted -= HandleGameRunning;
    }

    // Dipanggil saat klik tombol Play di Main Menu
    private void HandleGameStart()
    {
        gameplayManager.StartGame();
        // Tidak perlu set UI di sini, karena StartGame akan memicu event OnGameStarted 
        // yang kemudian ditangkap oleh HandleGameRunning di bawah ini.
    }

    // Dipanggil otomatis oleh event OnGameStarted (saat Next, Restart, atau StartGame)
    private void HandleGameRunning()
    {
        uiMainController.SetActive(false);
        uiEndgameController.SetActive(false);
        uiGameplayController.SetActive(true);
    }

    // Dipanggil otomatis oleh event OnGameEnded (saat Waktu Habis atau Finish)
    private void HandleGameEnded()
    {
        uiGameplayController.SetActive(false);
        uiEndgameController.SetActive(true);
    }

    // Dipanggil saat klik tombol Home di Endgame Menu
    private void HandleHomeButtonPressed()
    {
        uiEndgameController.SetActive(false);
        uiMainController.SetActive(true);

        // (Optional) Jika kamu mau menghapus map/karakter saat ke home menu, 
        // kamu bisa buat fungsi gameplayManager.ClearLevel() dan panggil di sini.
        // Tapi saat ini akan aman karena saat klik StartGame map lama akan dihancurkan.
    }

    // Dipanggil saat klik tombol Next di Endgame Menu
    private void HandleNextButtonPressed()
    {
        gameplayManager.NextLevel();
        // UI akan otomatis pindah karena NextLevel() memicu OnGameStarted -> HandleGameRunning()
    }

    // Dipanggil saat klik tombol Restart di Endgame Menu
    private void HandleRestartButtonPressed()
    {
        gameplayManager.RestartGame();
        // UI akan otomatis pindah karena RestartGame() memicu OnGameStarted -> HandleGameRunning()
    }
}