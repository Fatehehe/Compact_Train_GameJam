using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfigData), menuName = "App/Data/Game Config Data")]
public class GameConfigData : ScriptableObject
{
    [Header("Swipe Settings")]
    public float SwipeMinDistance = 50f;
    public float TapMaxDuration = 0.2f;
    public float LongPressMinDuration = 0.5f;

    [Header("Character Movement Fisik")]
    public float characterMinimumSpeed = 1f;
    public float characterMaximumSpeed = 10f;

    [Header("Character Lane System")]
    [Tooltip("Jarak antar lajur dalam satuan meter")]
    public float characterLaneWidth = 2.5f;
    [Tooltip("Seberapa cepat karakter berpindah lajur ke kanan/kiri")]
    public float characterLaneSwitchSpeed = 10f;
    [Tooltip("1 = 3 Lajur (-1, 0, 1). 2 = 5 Lajur (-2, -1, 0, 1, 2)")]
    public int maxLaneIndex = 1;

    [Header("Character Animation Blend")]
    [Tooltip("Nilai awal dan minimal blend tree vertikal (Lari Santai)")]
    public float minVerticalBlend = 0.25f;
    [Tooltip("Batas maksimal blend tree vertikal (Sprint penuh)")]
    public float maxVerticalBlend = 1f;
    [Tooltip("Penambahan/pengurangan kecepatan per swipe atas/bawah")]
    public float swipeVerticalStep = 0.25f;

    [Header("Animation Smoothness")]
    public float animatorDampTime = 0.1f;
    public float crossFadeDuration = 0.1f;

    [Header("Input & Game Feel")]
    public float scrollSensitivity = 10f;
    public float rotateSensitivity = 0.2f;
    public float dragThreshold = 25f;
    public float holdDelay = 0.2f;
    public float holdDuration = 0.5f;
    public float holdMoveTolerance = 5f;

    [Header("Artefact Assembly")]
    public float socketSnapDistance = 2f;
    public float emptySlotDistance = 7f;
    // public float assembleSnapDistance = 2f;
    public float recenterAnimDuration = 0.5f;

    [Header("Inspection Camera")]
    public float inspectionMinDistance = 2f;
    public float inspectionZoomSpeed = 0.02f;
    public float inspectionSmoothTime = 0.1f;
    public float inspectionResetDuration = 1f;
    public float inspectionPinchSensitivity = 0.05f;

    [Header("Tool Settings")]
    public float toolTipOffset = 135f;

    [Header("System Settings")]
    public float autoSaveCooldown = 1.0f;
    public float minLoadingScreenDuration = 2.0f;
    public float bgmFadeDuration = 2.0f;

    [Header("Tutorial Settings")]
    public string tutorialItemId = "Artefact_Coin";
    public string tutorialSecondItemId = "Artefact_Keris";


    [Header("Social Links")]
    public string steamURL;
    public string discordURL = "https://discord.gg/YOUR_INVITE_CODE";
    public string instagramURL = "https://www.instagram.com/YOUR_ACCOUNT_NAME";
}