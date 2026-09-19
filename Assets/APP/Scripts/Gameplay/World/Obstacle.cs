using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private readonly float knockback = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (other.TryGetComponent(out Sense sense))
        {
            sense.DealDamage(knockback);

            ObstacleDisappear();
        }
    }

    private void ObstacleDisappear()
    {
        Destroy(gameObject);
    }
}