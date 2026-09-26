using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public event Action<float> OnTakeDamage;
    public event Action OnCheckPoint;
    public event Action OnFinishReached;

    [SerializeField] private float defaultKnockback = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDestroyable destroyable))
        {
            OnTakeDamage?.Invoke(destroyable.GetKnockBack());
            destroyable.DestroyObject();
        }
        else if (other.TryGetComponent(out ICheckPoint checkpoint))
        {
            checkpoint.OnCheckPoint();
            if (checkpoint.HasTriggered()) { OnCheckPoint?.Invoke(); }
        }
        else if (other.gameObject.CompareTag("FinishPoint"))
        {
            OnFinishReached?.Invoke();
        }
    }
}