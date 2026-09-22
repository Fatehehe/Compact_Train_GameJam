using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private readonly List<IEnemy> detectedPushableEnemies = new();

    public event Action OnAttacked;

    private void Start()
    {
        detectedPushableEnemies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEnemy enemy) && !enemy.IsKnockedOut)
        {
            if (enemy.IsPushable)
            {
                if (!detectedPushableEnemies.Contains(enemy))
                {
                    detectedPushableEnemies.Add(enemy);
                }
            }
            else if (enemy.IsSwipeable)
            {
                OnAttacked?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IEnemy enemy))
        {
            if (enemy.IsPushable)
            {
                detectedPushableEnemies.Remove(enemy);
            }
        }
    }

    private IEnemy GetActivePushableEnemy()
    {
        detectedPushableEnemies.RemoveAll(e => e == null || e.Equals(null) || e.IsKnockedOut);

        if (detectedPushableEnemies.Count > 0)
        {
            return detectedPushableEnemies[detectedPushableEnemies.Count - 1];
        }

        return null;
    }

    public bool IsDetectingPushableEnemy()
    {
        return GetActivePushableEnemy() != null;
    }

    public bool PushActiveEnemy()
    {
        IEnemy pushable = GetActivePushableEnemy();
        if (pushable != null)
        {
            if (pushable.OnTakeDamage())
            {
                if (pushable.IsKnockedOut) detectedPushableEnemies.Remove(pushable);
                return true;
            }
        }
        return false;
    }

}