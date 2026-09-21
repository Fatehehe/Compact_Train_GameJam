using UnityEngine;

public class CheckPoint : MonoBehaviour, ICheckPoint
{
    [SerializeField] private Collider myCollider;
    public bool hasTriggered = false;
    public bool HasTriggered() => hasTriggered;

    public void OnCheckPoint()
    {
        if (hasTriggered) { return; }
        hasTriggered = true;
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