using System;

public static class GameEvents
{
    public static Action<string> OnPlayerSwipe;
    public static Action<float> OnMissHit;
    public static Action OnPlay;

    public static Action<bool> OnCheckpointStateChanged; // Untuk nyalakan/matikan "Tap Tap!"

    // Mengirim: Arah ("Left","Right","Down"), Progress Fill (0 ke 1), di SweetSpot?, Terlalu Dekat?
    public static Action<string, float, bool, bool> OnEnemyApproachUpdate;
    public static Action OnEnemyClear; // Untuk reset semua indikator

    // Tambahkan baris ini di dalam class GameEvents
    public static Action<float> OnTimerUpdated;
    public static Action OnComboHit;
    public static Action OnComboMiss;
}