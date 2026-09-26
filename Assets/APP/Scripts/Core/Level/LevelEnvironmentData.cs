using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnvironment", menuName = "Game/Level Environment Data")]
public class LevelEnvironmentData : ScriptableObject
{
    [Header("Environment Prefab")]
    public GameObject environmentPrefab;

    public Vector3 environmentSpawnPosition = Vector3.zero;

    [Header("Gameplay Positions")]
    public Vector3 playerStartPosition;
    public Vector3 finishPosition;

    [Header("Enemy Positions")]
    public Vector3[] pushEnemyPositions;


    [Header("Spawn Enemy Positions")]
    public Vector3[] spawnEnemyPositions;
    public GameObject spawnEnemyPrefab;

}