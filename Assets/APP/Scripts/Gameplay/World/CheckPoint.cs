using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (other.TryGetComponent(out ObstacleDetector ObstacleDetector))
        {
            ObstacleDetector.CheckPoint();

            ObstacleDisappear();
        }
    }

    private void ObstacleDisappear()
    {
        myCollider.enabled = false;
    }
}
