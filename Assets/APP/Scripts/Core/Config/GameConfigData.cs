using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfigData), menuName = "App/Data/Game Config Data")]
public class GameConfigData : ScriptableObject
{
    [Header("Swipe Settings")]
    public float SwipeMinDistance = 50f;
    public float TapMaxDuration = 0.2f;

    [Header("Character Physical Movement")] // "Fisik" diubah ke "Physical"
    public float characterMinimumSpeed = 1f;
    public float characterMaximumSpeed = 10f;

    [Header("Character Lane System")]
    [Tooltip("Distance between lanes in meters")]
    public float characterLaneWidth = 2.5f;
    [Tooltip("How fast the character switches to the left/right lanes")]
    public float characterLaneSwitchSpeed = 10f;
    [Tooltip("1 = 3 Lanes (-1, 0, 1). 2 = 5 Lanes (-2, -1, 0, 1, 2)")]
    public int maxLaneIndex = 1;

    [Header("Character Animation Blend")]
    [Tooltip("Initial and minimum vertical blend tree value (Jogging)")]
    public float minVerticalBlend = 0.25f;
    [Tooltip("Maximum vertical blend tree limit (Full sprint)")]
    public float maxVerticalBlend = 1f;
    [Tooltip("Speed increment/decrement per up/down swipe")]
    public float verticalStep = 0.25f;

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

    [Header("System Settings")]
    public float autoSaveCooldown = 1.0f;
    public float minLoadingScreenDuration = 2.0f;
    public float bgmFadeDuration = 2.0f;
}