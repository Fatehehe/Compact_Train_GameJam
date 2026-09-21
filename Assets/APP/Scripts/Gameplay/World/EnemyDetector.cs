using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    private readonly List<IEnemy> detectedEnemies = new();
    public event Action OnPulled;
    public event Action OnPushing;

    private IEnemy currentActiveEnemy;

    private void OnEnable()
    {
        detectedEnemies.Clear();
        currentActiveEnemy = null;
    }

    private void Update()
    {
        for (int i = detectedEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = detectedEnemies[i];
            if (enemy == null || enemy.Equals(null) || enemy.IsKnockedOut)
            {
                detectedEnemies.RemoveAt(i);
            }
        }

        IEnemy latestEnemy = detectedEnemies.Count > 0 ? detectedEnemies[detectedEnemies.Count - 1] : null;

        if (latestEnemy != currentActiveEnemy)
        {
            currentActiveEnemy = latestEnemy;

            if (currentActiveEnemy.IsSwipeable)
            {
                OnPulled?.Invoke();
            }
            else if (currentActiveEnemy.IsPushable)
            {
                OnPushing?.Invoke();
            }
        }
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

    public bool AttackActiveEnemy()
    {
        if (currentActiveEnemy != null && !currentActiveEnemy.IsKnockedOut)
        {
            return currentActiveEnemy.OnTakeDamage();
        }
        return false;
    }
}