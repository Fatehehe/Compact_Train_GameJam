using UnityEngine;

public class EnemyAnimationFunctions : MonoBehaviour
{
    public void AttackCompleted()
    {
        Debug.Log("Attack completed");
    }

    public void AnimationCompleted()
    {
        EnemyEvents.OnAnimationCompleted?.Invoke();
    }
}