using UnityEngine;
using TMPro;

public class UIGameplayController : BaseMenuController
{
    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Tap Tap UI")]
    [SerializeField] private GameObject tapContainer;
    [SerializeField] private TextMeshProUGUI tapText;
    [SerializeField] private float pulseSpeed = 10f;
    private bool isTappingPhase = false;

    [Header("Swipe Indicators (IndicatorUI)")]
    [SerializeField] private IndicatorUI leftIndicator;
    [SerializeField] private IndicatorUI rightIndicator;
    [SerializeField] private IndicatorUI bottomIndicator;

    private IndicatorUI currentActiveIndicator;
    private IndicatorUI lastActiveIndicator; // [FIXED] Ingatan indikator terakhir

    protected override void Awake()
    {
        base.Awake();

        GameEvents.OnCheckpointStateChanged += HandleCheckpointState;
        GameEvents.OnEnemyApproachUpdate += HandleEnemyApproach;
        GameEvents.OnEnemyClear += ResetAllIndicators;
        GameEvents.OnTimerUpdated += UpdateTimerText;
        GameEvents.OnComboMiss += HandleMissHit;
        GameEvents.OnComboHit += HandleComboHit;

        ResetAllIndicators();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.OnCheckpointStateChanged -= HandleCheckpointState;
        GameEvents.OnEnemyApproachUpdate -= HandleEnemyApproach;
        GameEvents.OnEnemyClear -= ResetAllIndicators;
        GameEvents.OnTimerUpdated -= UpdateTimerText;
        GameEvents.OnComboMiss -= HandleMissHit;
        GameEvents.OnComboHit -= HandleComboHit;
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

    private void UpdateTimerText(float timeRemaining)
    {
        if (!IsActive || timerText == null) return;
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";
        timerText.color = timeRemaining <= 10f ? Color.red : Color.white;
    }

    private void HandleCheckpointState(bool isAtCheckpoint)
    {
        isTappingPhase = !isAtCheckpoint;
        if (tapContainer != null) tapContainer.SetActive(isTappingPhase);
    }

    private void HandleEnemyApproach(string direction, float progress, bool isSweetSpot, bool isTooClose)
    {
        if (!IsActive) return;
        if (isTooClose) return;

        IndicatorUI targetIndicator = null;
        if (direction == "Left") targetIndicator = leftIndicator;
        else if (direction == "Right") targetIndicator = rightIndicator;
        else if (direction == "Down") targetIndicator = bottomIndicator;

        if (currentActiveIndicator != null && currentActiveIndicator != targetIndicator)
        {
            currentActiveIndicator.Hide();
        }

        if (targetIndicator != null)
        {
            // [FIXED] Panggil Show() lagi jika arahnya berubah, ATAU jika GameObject mati karena habis animasi MISS
            if (currentActiveIndicator != targetIndicator || !targetIndicator.gameObject.activeSelf)
            {
                targetIndicator.Show();
                currentActiveIndicator = targetIndicator;
                lastActiveIndicator = targetIndicator;
            }
            targetIndicator.UpdateProgress(progress, isSweetSpot);
        }
    }

    private void HandleMissHit()
    {
        Debug.Log("HandleMissHit");
        // [FIXED] Coba pakai indikator saat ini, kalau null (karena telat mukul), pakai ingatan terakhir
        IndicatorUI target = currentActiveIndicator != null ? currentActiveIndicator : lastActiveIndicator;

        if (target != null)
        {
            target.ShowMiss();
            // JANGAN DIBUAT NULL di sini agar indikator tidak ke-reset!
        }
    }

    private void HandleComboHit()
    {
        IndicatorUI target = currentActiveIndicator != null ? currentActiveIndicator : lastActiveIndicator;

        if (target != null)
        {
            target.ShowCombo();
            // JANGAN DIBUAT NULL di sini
        }
    }

    public void ResetAllIndicators()
    {
        if (leftIndicator != null) leftIndicator.Hide();
        if (rightIndicator != null) rightIndicator.Hide();
        if (bottomIndicator != null) bottomIndicator.Hide();
        currentActiveIndicator = null;
    }
}