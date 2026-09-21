using UnityEngine;

public class CheckPoint : MonoBehaviour, ICheckPoint
{
    [SerializeField] private Collider myCollider;

    [SerializeField] private Transform LeftTransform;
    [SerializeField] private Transform RightTransform;
    [SerializeField] private Transform BehindTransform;

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