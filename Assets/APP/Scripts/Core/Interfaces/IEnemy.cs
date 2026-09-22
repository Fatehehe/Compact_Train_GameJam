using UnityEngine;

public interface IEnemy
{
    // int HP { get; }
    bool IsKnockedOut { get; }
    bool IsPushable { get; }
    bool IsSwipeable { get; }
    void OnChasingPerformed(Vector3 position);
    void OnStopChasing();
    bool OnTakeDamage();
    void OnKnockedOut();
    void OnTargetReached();
}