using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;
    public event Action OnPulled;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public void Pull()
    {
        OnPulled?.Invoke();
    }
}