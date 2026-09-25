using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndgameController : BaseMenuController
{
    [SerializeField] private Button buttonRestart;
    [SerializeField] private Button buttonNext;
    [SerializeField] private GameObject winInfo;
    [SerializeField] private GameObject loseInfo;
    [SerializeField] private TextMeshProUGUI levelText;

    public event Action OnRestartButtonPressed;
    public event Action OnNextButtonPressed;

    protected override void Awake()
    {
        base.Awake();
        if (buttonRestart != null)
        {
            buttonRestart.onClick.AddListener(RestartButtonPressed);
        }
        if (buttonNext != null)
        {
            buttonNext.onClick.AddListener(NextButtonPressed);
        }
    }

    private void NextButtonPressed()
    {
        OnNextButtonPressed?.Invoke();
    }

    private void RestartButtonPressed()
    {
        OnRestartButtonPressed?.Invoke();
    }

    public void SetResultText(bool isWinning, string level)
    {
        if (isWinning)
        {
            winInfo.SetActive(true);
            loseInfo.SetActive(false);

            // Jika menang, munculkan tombol Next
            if (buttonNext != null) buttonNext.gameObject.SetActive(true);
        }
        else
        {
            winInfo.SetActive(false);
            loseInfo.SetActive(true);

            // Jika kalah, sembunyikan tombol Next
            if (buttonNext != null) buttonNext.gameObject.SetActive(false);
        }

        // Pastikan tombol Restart selalu muncul (baik saat menang maupun kalah)
        if (buttonRestart != null) buttonRestart.gameObject.SetActive(true);

        levelText.SetText("Day " + level);
    }
}