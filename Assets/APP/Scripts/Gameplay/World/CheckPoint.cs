using UnityEngine;

public class CheckPoint : MonoBehaviour, ICheckPoint
{
    public bool hasTriggered = false;
    public bool HasTriggered() => hasTriggered;

    public void OnCheckPoint()
    {
        if (hasTriggered) { return; }
        hasTriggered = true;
    }
}