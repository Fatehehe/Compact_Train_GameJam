using System; // Tambahkan ini untuk event Action
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private readonly List<Collider> alreadyCollidedWith = new();
    private readonly int tapDamage = 1;

    // Event ini akan terpanggil saat musuh terdekat ditemukan, berubah, atau hilang
    public event Action<Enemy> OnClosestEnemyChanged;

    private Enemy currentClosestEnemy;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
        currentClosestEnemy = null;
    }

    private void Update()
    {
        // Cek terus menerus siapa musuh terdekat
        Enemy closest = GetClosestEnemy();

        // Jika musuh terdekat berubah (atau musuh baru masuk/keluar)
        if (closest != currentClosestEnemy)
        {
            currentClosestEnemy = closest;
            OnClosestEnemyChanged?.Invoke(currentClosestEnemy); // Panggil event
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }
        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (alreadyCollidedWith.Contains(other))
        {
            alreadyCollidedWith.Remove(other);
        }
    }

    // Fungsi khusus untuk mencari musuh terdekat saat ini
    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        for (int i = alreadyCollidedWith.Count - 1; i >= 0; i--)
        {
            Collider col = alreadyCollidedWith[i];

            // Cek jika null atau objek sudah tidak aktif (mati)
            if (col == null || !col.gameObject.activeInHierarchy)
            {
                alreadyCollidedWith.RemoveAt(i);
                continue;
            }

            if (col.TryGetComponent<Enemy>(out Enemy enemy))
            {
                float dSqrToTarget = (col.transform.position - currentPosition).sqrMagnitude;

                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    public bool AttackEnemy()
    {
        // Jika ada musuh terdekat
        if (currentClosestEnemy != null)
        {
            Debug.Log($"Menyerang musuh: {currentClosestEnemy.name} dengan damage {tapDamage}");

            // Asumsi: DealDamage di script Enemy diubah untuk me-return bool (true = mati, false = hidup)
            // Jika DealDamage kamu saat ini bertipe void, kamu perlu mengubahnya (lihat penjelasan di bawah)
            bool isKilled = currentClosestEnemy.DealDamage(tapDamage);

            return isKilled; // Kembalikan true jika mati, false jika belum
        }

        // Jika tidak ada musuh, return false
        return false;
    }
}