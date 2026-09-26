using UnityEngine;
using TMPro;
using DG.Tweening; // Tambahkan ini untuk DOTween

public class UIGameplayController : BaseMenuController
{
    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Tap Tap UI")]
    [SerializeField] private GameObject tapContainer;
    [SerializeField] private TextMeshProUGUI tapText;

    // Variabel diganti agar lebih sesuai dengan DOTween
    [SerializeField] private float tapPulseScale = 1.2f;
    [SerializeField] private float tapPulseDuration = 0.2f;
    private bool isPushingPhase = false;
    private Tween tapTween; // Menyimpan referensi animasi

    [Header("Swipe Indicators (IndicatorUI)")]
    [SerializeField] private IndicatorUI leftIndicator;
    [SerializeField] private IndicatorUI rightIndicator;
    [SerializeField] private IndicatorUI bottomIndicator;
    [SerializeField] private TextMeshProUGUI levelText;

    private IndicatorUI currentActiveIndicator;
    private IndicatorUI lastActiveIndicator;

    protected override void Awake()
    {
        base.Awake();

        GameEvents.OnPushing += HandleOnPushing;
        GameEvents.OnEnemyApproachUpdate += HandleEnemyApproach;
        GameEvents.OnEnemyClear += ResetAllIndicators;
        GameEvents.OnTimerUpdated += UpdateTimerText;
        GameEvents.OnComboMiss += HandleMissHit;
        GameEvents.OnComboHit += HandleComboHit;

        tapContainer.SetActive(false);
        ResetAllIndicators();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameEvents.OnPushing -= HandleOnPushing;
        GameEvents.OnEnemyApproachUpdate -= HandleEnemyApproach;
        GameEvents.OnEnemyClear -= ResetAllIndicators;
        GameEvents.OnTimerUpdated -= UpdateTimerText;
        GameEvents.OnComboMiss -= HandleMissHit;
        GameEvents.OnComboHit -= HandleComboHit;

        // Hentikan animasi agar tidak error saat objek dihancurkan
        tapTween?.Kill();
    }

    // FUNGSI UPDATE DIHAPUS - DOTween sudah menangani animasinya otomatis

    private void UpdateTimerText(float timeRemaining)
    {
        if (!IsActive || timerText == null) return;
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString() + "s";
        timerText.color = timeRemaining <= 10f ? Color.red : Color.white;
    }

    private void HandleOnPushing(bool isPushing)
    {
        isPushingPhase = !isPushing;
        if (tapContainer != null)
        {
            tapContainer.SetActive(isPushing);

            if (isPushing && tapText != null)
            {
                // Mulai Animasi DOTween
                if (tapTween == null || !tapTween.IsActive())
                {
                    tapText.transform.localScale = Vector3.one;
                    tapTween = tapText.transform.DOScale(Vector3.one * tapPulseScale, tapPulseDuration)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
            }
            else
            {
                // Matikan Animasi dan reset ukuran
                tapTween?.Kill();
                if (tapText != null) tapText.transform.localScale = Vector3.one;
            }
        }
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
        IndicatorUI target = currentActiveIndicator != null ? currentActiveIndicator : lastActiveIndicator;

        if (target != null)
        {
            target.ShowMiss();
        }
    }

    private void HandleComboHit()
    {
        IndicatorUI target = currentActiveIndicator != null ? currentActiveIndicator : lastActiveIndicator;

        if (target != null)
        {
            target.ShowCombo();
        }
    }

    public void ResetAllIndicators()
    {
        if (leftIndicator != null) leftIndicator.Hide();
        if (rightIndicator != null) rightIndicator.Hide();
        if (bottomIndicator != null) bottomIndicator.Hide();
        currentActiveIndicator = null;
        lastActiveIndicator = null;
    }

    public void SetLevelText(string level)
    {
        levelText.SetText("Day " + level);
    }
}