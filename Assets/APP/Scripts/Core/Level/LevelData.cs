using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Environment Data")]
    public LevelEnvironmentData levelEnvironment;

    [Header("Level Mechanics")]
    public float levelTimer = 60f;
    public float randomMinTimeSpawn = 0f;
    public float randomMaxTimeSpawn = 1.5f;
    public GameObject pushEnemyPrefab;

    [Header("Hit Mechanics (Sweet Spot)")]
    public float minHitDistance = 1.0f;
    public float maxHitDistance = 3.0f;
    public float timePenalty = 2.0f;
}