using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) { return; }

        if (other == myCollider) { return; }

        if (other.TryGetComponent(out ObstacleDetector obstacleDetector))
        {
            hasTriggered = true;

            Debug.Log($"CheckPoint triggered by: {other.name}");
            obstacleDetector.CheckPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == myCollider) { return; }
        if (hasTriggered)
        {
            hasTriggered = false;
        }
    }
}