using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public event Action<float> OnTakeDamage;
    public event Action OnCheckPoint;

    [SerializeField] private float defaultKnockback = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDestroyable destroyable))
        {
            OnTakeDamage?.Invoke(defaultKnockback);
            destroyable.DestroyObject();
        }
        else if (other.TryGetComponent(out ICheckPoint checkpoint))
        {
            checkpoint.OnCheckPoint();
            if (checkpoint.HasTriggered()) { OnCheckPoint?.Invoke(); }
        }
    }
}