using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IndicatorUI : MonoBehaviour
{
    [Header("UI References (Tarik dari Children)")]
    [SerializeField] private GameObject baseIndicator;
    [SerializeField] private Image fillIndicator;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI missText;

    [Header("Settings")]
    [SerializeField] private Color fillingColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private Color sweetSpotColor = Color.green;
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseDuration = 0.2f;

    private Tween pulseTween;
    private bool isCurrentlySweetSpot = false;
    private bool isShowingResult = false;

    private void Awake()
    {
        ForceHide();
    }

    public void Show()
    {
        // [FIXED] Jangan di-return! Kalau Show dipanggil, putus paksa semua animasi dan reset!
        isShowingResult = false;
        KillAllTweens();

        gameObject.SetActive(true);

        baseIndicator.SetActive(true);
        fillIndicator.gameObject.SetActive(true);
        fillIndicator.color = fillingColor;
        fillIndicator.fillAmount = 0f;

        if (missText != null) missText.gameObject.SetActive(false);
        if (comboText != null) comboText.gameObject.SetActive(false);

        isCurrentlySweetSpot = false;
        ResetTransform();
    }

    public void Hide()
    {
        if (isShowingResult) return;
        ForceHide();
    }

    private void ForceHide()
    {
        isShowingResult = false;
        KillAllTweens();
        gameObject.SetActive(false);
    }

    public void UpdateProgress(float progress, bool isSweetSpot)
    {
        if (isShowingResult) return;

        fillIndicator.fillAmount = progress;

        if (isSweetSpot && !isCurrentlySweetSpot)
        {
            isCurrentlySweetSpot = true;
            fillIndicator.color = sweetSpotColor;

            KillAllTweens();
            pulseTween = transform.DOScale(Vector3.one * pulseScale, pulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else if (!isSweetSpot && isCurrentlySweetSpot)
        {
            isCurrentlySweetSpot = false;
            fillIndicator.color = fillingColor;
            ResetTransform();
        }
    }

    public void ShowMiss()
    {
        Debug.Log("indicator Miss!");
        isShowingResult = true;
        gameObject.SetActive(true);

        KillAllTweens();
        ResetTransform();

        baseIndicator.SetActive(false);
        fillIndicator.gameObject.SetActive(false);
        if (comboText != null) comboText.gameObject.SetActive(false);

        if (missText != null)
        {
            missText.gameObject.SetActive(true);
            missText.transform.localScale = Vector3.one;
            missText.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f, 10, 1)
                .OnComplete(() =>
                {
                    ForceHide();
                });
        }
    }

    public void ShowCombo()
    {
        isShowingResult = true;
        gameObject.SetActive(true);

        KillAllTweens();
        ResetTransform();

        baseIndicator.SetActive(false);
        fillIndicator.gameObject.SetActive(false);
        if (missText != null) missText.gameObject.SetActive(false);

        if (comboText != null)
        {
            comboText.gameObject.SetActive(true);
            comboText.transform.localScale = Vector3.one;
            comboText.transform.DOPunchScale(Vector3.one * 0.6f, 0.3f, 10, 1)
                .OnComplete(() =>
                {
                    ForceHide();
                });
        }
    }

    private void ResetTransform()
    {
        KillAllTweens();
        transform.localScale = Vector3.one;
    }

    private void KillAllTweens()
    {
        if (pulseTween != null)
        {
            pulseTween.Kill();
            pulseTween = null;
        }
        transform.DOKill();

        if (missText != null) missText.transform.DOKill();
        if (comboText != null) comboText.transform.DOKill();
    }

    private void OnDisable()
    {
        KillAllTweens();
        isShowingResult = false;
    }
}