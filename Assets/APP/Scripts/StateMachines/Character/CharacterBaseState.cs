using UnityEngine;

public abstract class CharacterBaseState : State
{
    protected CharacterStateMachine stateMachine;
    public CharacterBaseState(CharacterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void HandleVerticalMovement(float deltaTime, float currentAnimVertical)
    {
        if (currentAnimVertical == 0f) return;

        float currentSpeed = Mathf.Lerp(
            stateMachine.Config.characterMinimumSpeed,
            stateMachine.Config.characterMaximumSpeed,
            currentAnimVertical
        );
        stateMachine.transform.Translate(Vector3.forward * currentSpeed * deltaTime);
    }

    protected void MoveForward(float deltaTime, float speed)
    {
        stateMachine.transform.Translate(Vector3.forward * speed * deltaTime);
    }

    protected void HandleHorizontalMovement(float deltaTime, int currentLane, int HorizontalHash)
    {
        float targetPositionX = currentLane * stateMachine.Config.characterLaneWidth;

        Vector3 currentPos = stateMachine.transform.position;

        float newX = Mathf.Lerp(currentPos.x, targetPositionX, deltaTime * stateMachine.Config.characterLaneSwitchSpeed);
        stateMachine.transform.position = new Vector3(newX, currentPos.y, currentPos.z);

        float distanceToTarget = targetPositionX - stateMachine.transform.position.x;
        float animHorizontal = Mathf.Clamp(distanceToTarget, -1f, 1f);

        stateMachine.Animator.SetFloat(HorizontalHash, animHorizontal, stateMachine.Config.animatorDampTime, deltaTime);
    }

    protected void MoveHorizontal(float deltaTime, int currentLane)
    {
        float targetPositionX = currentLane * stateMachine.Config.characterLaneWidth;
        Vector3 currentPos = stateMachine.transform.position;
        float newX = Mathf.Lerp(currentPos.x, targetPositionX, deltaTime * stateMachine.Config.characterLaneSwitchSpeed);
        stateMachine.transform.position = new Vector3(newX, currentPos.y, currentPos.z);
        float distanceToTarget = targetPositionX - stateMachine.transform.position.x;
        float animHorizontal = Mathf.Clamp(distanceToTarget, -1f, 1f);
    }
}
