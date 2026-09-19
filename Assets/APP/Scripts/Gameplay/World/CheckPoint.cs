using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private readonly float knockback = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (other.TryGetComponent(out Sense sense))
        {
            sense.CheckPoint();

            ObstacleDisappear();
        }
    }

    private void ObstacleDisappear()
    {
        Destroy(gameObject);
    }
}
