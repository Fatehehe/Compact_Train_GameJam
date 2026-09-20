using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int health;

    public event Action OnTakeDamage;
    public event Action OnTakeOut;

    public bool IsDead => health == 0;

    void Start()
    {
        health = maxHealth;
    }

    public bool DealDamage(int damage)
    {
        // Jika sudah mati, tidak perlu kurangi health atau panggil event lagi
        if (IsDead) return true;

        // Kurangi health, pastikan tidak kurang dari 0
        health = Mathf.Max(0, health - damage);

        // Panggil event terkena damage
        OnTakeDamage?.Invoke();

        // Cek apakah musuh mati setelah kena damage
        if (health == 0)
        {
            OnTakeOut?.Invoke(); // Panggil event mati
            return true;         // Return true karena musuh baru saja mati
        }

        return false; // Return false karena musuh masih hidup
    }
}