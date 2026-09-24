using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using DG.Tweening; // Pastikan DOTween dipanggil

public class UIMainController : BaseMenuController
{
    [Header("Main Menu UI")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonCredit;
    [SerializeField] private GameObject mainImage;
    [SerializeField] private GameObject dateImage;

    [Header("Credit UI")]
    [SerializeField] private GameObject creditPanel;
    [SerializeField] private Button buttonCloseCredit;

    [Header("Animation Settings")]
    [SerializeField] private float animDuration = 0.3f; // Durasi animasi

    private bool isAnimating = false; // Kunci agar tidak bisa spam klik saat animasi jalan

    public event Action OnGameStart;

    protected override void Awake()
    {
        base.Awake();

        if (buttonStart != null) buttonStart.onClick.AddListener(OnStartGame);
        if (buttonCredit != null) buttonCredit.onClick.AddListener(ShowCredit);
        if (buttonCloseCredit != null) buttonCloseCredit.onClick.AddListener(HideCredit);

        // Setup kondisi awal (Main Menu tampil, Credit disembunyikan)
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
            creditPanel.transform.localScale = Vector3.zero; // Set scale ke 0
        }

        if (buttonStart != null) buttonStart.gameObject.SetActive(true);
        if (buttonCredit != null) buttonCredit.gameObject.SetActive(true);
        if (mainImage != null) mainImage.SetActive(true);
        if (dateImage != null) dateImage.SetActive(true);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (buttonStart != null) buttonStart.onClick.RemoveListener(OnStartGame);
        if (buttonCredit != null) buttonCredit.onClick.RemoveListener(ShowCredit);
        if (buttonCloseCredit != null) buttonCloseCredit.onClick.RemoveListener(HideCredit);

        // Bersihkan semua animasi yang berjalan di object ini jika di-destroy
        DOTween.Kill(this);
    }

    private void OnStartGame()
    {
        if (isAnimating) return;
        OnGameStart?.Invoke();
    }

    private void ShowCredit()
    {
        if (isAnimating) return;
        isAnimating = true;

        // Bikin urutan animasi (Sequence)
        Sequence seq = DOTween.Sequence();

        // 1. Animasikan Main Menu mengecil secara bersamaan
        if (buttonStart != null) seq.Join(buttonStart.transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InBack));
        if (buttonCredit != null) seq.Join(buttonCredit.transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InBack));
        if (mainImage != null) seq.Join(mainImage.transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InBack));

        seq.OnComplete(() =>
        {
            // 2. Matikan Game Object Main Menu setelah mengecil
            if (buttonStart != null) buttonStart.gameObject.SetActive(false);
            if (buttonCredit != null) buttonCredit.gameObject.SetActive(false);
            if (mainImage != null) mainImage.SetActive(false);
            if (dateImage != null) dateImage.SetActive(false);


            // 3. Nyalakan Credit Panel & animasikan membesar
            if (creditPanel != null)
            {
                creditPanel.SetActive(true);
                creditPanel.transform.localScale = Vector3.zero;

                creditPanel.transform.DOScale(Vector3.one, animDuration)
                    .SetEase(Ease.OutBack) // Efek mantul
                    .OnComplete(() => isAnimating = false); // Buka kunci setelah selesai
            }
            else
            {
                isAnimating = false;
            }
        });
    }

    private void HideCredit()
    {
        if (isAnimating) return;
        isAnimating = true;

        if (creditPanel != null)
        {
            // 1. Animasikan Credit Panel mengecil
            creditPanel.transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                creditPanel.SetActive(false);

                // 2. Nyalakan kembali Main Menu dan animasikan membesar bersamaan
                if (buttonStart != null)
                {
                    buttonStart.gameObject.SetActive(true);
                    buttonStart.transform.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack);
                }
                if (buttonCredit != null)
                {
                    buttonCredit.gameObject.SetActive(true);
                    buttonCredit.transform.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack);
                }
                if (mainImage != null)
                {
                    mainImage.SetActive(true);
                    mainImage.transform.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack);
                }
                if (dateImage != null)
                {
                    dateImage.SetActive(true);
                    dateImage.transform.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack);
                }


                // 3. Buka kunci setelah durasi animasi membesar selesai
                DOVirtual.DelayedCall(animDuration, () => isAnimating = false);
            });
        }
    }
}