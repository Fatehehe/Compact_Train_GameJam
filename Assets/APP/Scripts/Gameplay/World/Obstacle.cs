using UnityEngine;

public class Obstacle : MonoBehaviour, IDestroyable
{
    private readonly float knockback = 10f;
    public float GetKnockBack() => knockback;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}