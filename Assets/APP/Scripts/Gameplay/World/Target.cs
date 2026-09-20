using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;
    public event Action OnPulled;
    public int pullHP = 5;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public void Pull(int pullDamage)
    {
        if (pullHP == 0) return;

        pullHP -= pullDamage;

        if (pullHP == 0)
        {
            OnPulled?.Invoke();
        }
    }

    public void ResetHp()
    {
        pullHP = 5;
    }
}