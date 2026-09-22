using UnityEngine;

public class EnemyAnimationFunctions : MonoBehaviour
{
    public void AttackCompleted()
    {
        Debug.Log("Attack completed");
    }

    public void AttackAnimationCompleted()
    {
        EnemyEvents.OnAttackAnimationCompleted?.Invoke();
    }
}