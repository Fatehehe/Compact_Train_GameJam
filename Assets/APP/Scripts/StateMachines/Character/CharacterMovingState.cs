using UnityEngine;

public class CharacterMovingState : CharacterBaseState
{
    private readonly int MovingBlendTreeHash = Animator.StringToHash("MovingBlendTree");
    private readonly int HorizontalHash = Animator.StringToHash("Horizontal");
    private readonly int VerticalHash = Animator.StringToHash("Vertical");

    private float targetVertical;
    private int currentLane = 0;

    public CharacterMovingState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        targetVertical = stateMachine.Config.minVerticalBlend;
        Debug.Log($"targetVertical: {targetVertical}");
        currentLane = 0;

        PlayerEvents.OnSwipeUpPerformed += HandleSwipeUp;
        PlayerEvents.OnSwipeDownPerformed += HandleSwipeDown;
        PlayerEvents.OnSwipeRightPerformed += HandleSwipeRight;
        PlayerEvents.OnSwipeLeftPerformed += HandleSwipeLeft;

        stateMachine.Animator.CrossFadeInFixedTime(MovingBlendTreeHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        HandleHorizontalMovement(deltaTime);
        stateMachine.Animator.SetFloat(VerticalHash, targetVertical, stateMachine.Config.animatorDampTime, deltaTime);
        MoveForward(deltaTime);
    }

    public override void Exit()
    {
        PlayerEvents.OnSwipeUpPerformed -= HandleSwipeUp;
        PlayerEvents.OnSwipeDownPerformed -= HandleSwipeDown;
        PlayerEvents.OnSwipeRightPerformed -= HandleSwipeRight;
        PlayerEvents.OnSwipeLeftPerformed -= HandleSwipeLeft;
    }

    private void HandleSwipeUp()
    {
        targetVertical = Mathf.Clamp(
            targetVertical + stateMachine.Config.swipeVerticalStep,
            stateMachine.Config.minVerticalBlend,
            stateMachine.Config.maxVerticalBlend
        );
    }

    private void HandleSwipeDown()
    {
        targetVertical = Mathf.Clamp(
            targetVertical - stateMachine.Config.swipeVerticalStep,
            stateMachine.Config.minVerticalBlend,
            stateMachine.Config.maxVerticalBlend
        );
    }

    private void HandleSwipeRight()
    {
        currentLane = Mathf.Clamp(currentLane + 1, -stateMachine.Config.maxLaneIndex, stateMachine.Config.maxLaneIndex);
    }

    private void HandleSwipeLeft()
    {
        currentLane = Mathf.Clamp(currentLane - 1, -stateMachine.Config.maxLaneIndex, stateMachine.Config.maxLaneIndex);
    }

    private void HandleHorizontalMovement(float deltaTime)
    {
        float targetPositionX = currentLane * stateMachine.Config.characterLaneWidth;

        Vector3 currentPos = stateMachine.transform.position;

        float newX = Mathf.Lerp(currentPos.x, targetPositionX, deltaTime * stateMachine.Config.characterLaneSwitchSpeed);
        stateMachine.transform.position = new Vector3(newX, currentPos.y, currentPos.z);

        float distanceToTarget = targetPositionX - stateMachine.transform.position.x;
        float animHorizontal = Mathf.Clamp(distanceToTarget, -1f, 1f);

        stateMachine.Animator.SetFloat(HorizontalHash, animHorizontal, stateMachine.Config.animatorDampTime, deltaTime);
    }

    private void MoveForward(float deltaTime)
    {
        float currentAnimVertical = stateMachine.Animator.GetFloat(VerticalHash);
        float currentSpeed = Mathf.Lerp(
            stateMachine.Config.characterMinimumSpeed,
            stateMachine.Config.characterMaximumSpeed,
            currentAnimVertical
        );
        Debug.Log($"currentAnimVertical: {currentAnimVertical}");
        stateMachine.transform.Translate(Vector3.forward * currentSpeed * deltaTime);
    }
}