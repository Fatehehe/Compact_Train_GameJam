using UnityEngine;

public class EnemyAnimationFunctions : MonoBehaviour
{
    public void AttackCompleted()
    {
        Debug.Log("Attack completed");
        EnemyEvents.OnAttackCompleted?.Invoke();
    }

    public void AttackAnimationCompleted()
    {
        EnemyEvents.OnAttackAnimationCompleted?.Invoke();
    }
}