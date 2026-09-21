using UnityEngine;

public class Obstacle : MonoBehaviour, IDestroyable
{
    [SerializeField] private Collider myCollider;
    private readonly float knockback = 10f;
    public float GetKnockBack() => knockback;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}