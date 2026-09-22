using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Environment Data")]
    public LevelEnvironmentData levelEnvironment;

    [Header("Level Mechanics")]
    public float levelTimer = 60f;
    public int checkpointEnemyCount = 5;
    public float randomMinTimeSpawn = 0f;
    public float randomMaxTimeSpawn = 5f;
    public GameObject pushEnemyPrefab;
}