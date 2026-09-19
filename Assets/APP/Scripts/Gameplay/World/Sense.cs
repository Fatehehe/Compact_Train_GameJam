using System;
using UnityEngine;

public class Sense : MonoBehaviour
{
    public event Action<float> OnTakeDamage;
    public event Action OnCheckPoint;

    public void DealDamage(float knockBack)
    {
        OnTakeDamage?.Invoke(knockBack);
    }

    public void CheckPoint()
    {
        OnCheckPoint?.Invoke();
    }
}
