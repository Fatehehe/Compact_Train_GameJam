using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameplayController : BaseMenuController
{
    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI timerText; // Referensi teks timer

    [Header("Tap Tap UI")]
    [SerializeField] private GameObject tapContainer;
    [SerializeField] private TextMeshProUGUI tapText;
    [SerializeField] private float pulseSpeed = 10f;
    private bool isTappingPhase = false;

    [Header("Swipe Indicators (Fill Images)")]
    [SerializeField] private Image leftIndicator;
    [SerializeField] private Image rightIndicator;
    [SerializeField] private Image bottomIndicator;

    [Header("Indicator Colors")]
    [SerializeField] private Color fillingColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private Color sweetSpotColor = Color.green;
    [SerializeField] private Color missedColor = Color.red;

    protected override void Awake()
    {
        base.Awake();

        GameEvents.OnCheckpointStateChanged += HandleCheckpointState;
        GameEvents.OnEnemyApproachUpdate += HandleEnemyApproach;
        GameEvents.OnEnemyClear += ResetAllIndicators;
        GameEvents.OnTimerUpdated += UpdateTimerText; // Dengarkan update waktu

        ResetAllIndicators();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.OnCheckpointStateChanged -= HandleCheckpointState;
        GameEvents.OnEnemyApproachUpdate -= HandleEnemyApproach;
        GameEvents.OnEnemyClear -= ResetAllIndicators;
        GameEvents.OnTimerUpdated -= UpdateTimerText;
    }

    private void Update()
    {
        if (!IsActive) return;

        if (isTappingPhase && tapText != null && tapContainer.activeSelf)
        {
            float scale = 1f + Mathf.PingPong(Time.time * pulseSpeed, 0.2f);
            tapText.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    // FUNGSI BARU: Mengubah Teks Timer
    private void UpdateTimerText(float timeRemaining)
    {
        if (!IsActive || timerText == null) return;

        // Membulatkan waktu ke atas agar rapi, misalnya: 59, 58, 57...
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";

        // Opsional: Bikin merah dan bergetar kalau waktu kurang dari 10 detik!
        if (timeRemaining <= 10f)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
    }

    private void HandleCheckpointState(bool isAtCheckpoint)
    {
        isTappingPhase = !isAtCheckpoint;
        if (tapContainer != null)
        {
            tapContainer.SetActive(isTappingPhase);
        }
    }

    private void HandleEnemyApproach(string direction, float progress, bool isSweetSpot, bool isTooClose)
    {
        if (!IsActive) return;

        ResetAllIndicators();

        Image activeIndicator = null;
        if (direction == "Left") activeIndicator = leftIndicator;
        else if (direction == "Right") activeIndicator = rightIndicator;
        else if (direction == "Down") activeIndicator = bottomIndicator;

        if (activeIndicator != null)
        {
            activeIndicator.gameObject.SetActive(true);
            activeIndicator.fillAmount = progress;

            if (isTooClose)
            {
                activeIndicator.color = missedColor;
                activeIndicator.transform.localScale = Vector3.one;
            }
            else if (isSweetSpot)
            {
                activeIndicator.color = sweetSpotColor;
                float pulse = 1f + Mathf.PingPong(Time.time * 15f, 0.2f);
                activeIndicator.transform.localScale = new Vector3(pulse, pulse, 1f);
            }
            else
            {
                activeIndicator.color = fillingColor;
                activeIndicator.transform.localScale = Vector3.one;
            }
        }
    }

    private void ResetAllIndicators()
    {
        if (leftIndicator != null) { leftIndicator.gameObject.SetActive(false); leftIndicator.fillAmount = 0; }
        if (rightIndicator != null) { rightIndicator.gameObject.SetActive(false); rightIndicator.fillAmount = 0; }
        if (bottomIndicator != null) { bottomIndicator.gameObject.SetActive(false); bottomIndicator.fillAmount = 0; }
    }
}