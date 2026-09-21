using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private Transform behindTransform;

    public Transform LeftSpawnPosition => leftTransform;
    public Transform RightSpawnPosition => rightTransform;
    public Transform BehindSpawnPosition => behindTransform;

    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    public Transform GetTransform => transform;

    public void SetSpawnerPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}