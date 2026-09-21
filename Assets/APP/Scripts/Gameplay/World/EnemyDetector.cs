using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private readonly List<IEnemy> detectedEnemies = new();

    private void OnEnable()
    {
        detectedEnemies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEnemy enemy))
        {
            if (!detectedEnemies.Contains(enemy) && !enemy.IsKnockedOut)
            {
                detectedEnemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IEnemy enemy))
        {
            if (detectedEnemies.Contains(enemy))
            {
                detectedEnemies.Remove(enemy);
            }
        }
    }

    public IEnemy GetLatestActiveEnemy()
    {
        for (int i = detectedEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = detectedEnemies[i];
            if (enemy == null || enemy.Equals(null) || enemy.IsKnockedOut)
            {
                detectedEnemies.RemoveAt(i);
            }
        }

        if (detectedEnemies.Count > 0)
        {
            return detectedEnemies[detectedEnemies.Count - 1];
        }

        return null;
    }

    public bool IsDetectingSwipeable()
    {
        IEnemy enemy = GetLatestActiveEnemy();
        return enemy != null && enemy.IsSwipeable;
    }

    public bool IsDetectingPushable()
    {
        IEnemy enemy = GetLatestActiveEnemy();
        return enemy != null && enemy.IsPushable;
    }

    public bool AttackActiveEnemy()
    {
        IEnemy enemy = GetLatestActiveEnemy();
        if (enemy != null)
        {
            if (enemy.OnTakeDamage())
            {
                detectedEnemies.Remove(enemy);
                return true;
            }
        }

        return false;
    }
}