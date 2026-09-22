using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Environment Data")]
    public LevelEnvironmentData levelEnvironment;

    [Header("Level Mechanics")]
    public float levelTimer = 60f;
    public int checkpointEnemyCount = 3;
    public GameObject pushEnemyPrefab;
}