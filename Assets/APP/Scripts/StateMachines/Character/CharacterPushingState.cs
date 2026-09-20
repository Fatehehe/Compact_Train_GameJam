using System;
using UnityEngine;

public class CharacterPushingState : CharacterBaseState
{
    private readonly int PushHash = Animator.StringToHash("Walk");

    public CharacterPushingState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("character state: CharacterPushingState");

        stateMachine.Animator.CrossFadeInFixedTime(PushHash, stateMachine.Config.crossFadeDuration);
        PlayerEvents.OnSwipeRightPerformed += HandleSwipeRight;
        PlayerEvents.OnSwipeLeftPerformed += HandleSwipeLeft;
        PlayerEvents.OnSwipeDownPerformed += HandleSwipeDown;
        PlayerEvents.OnTapPerformed += HandleTap;
    }

    public override void Exit()
    {
        PlayerEvents.OnSwipeRightPerformed -= HandleSwipeRight;
        PlayerEvents.OnSwipeLeftPerformed -= HandleSwipeLeft;
        PlayerEvents.OnSwipeDownPerformed -= HandleSwipeDown;
        PlayerEvents.OnTapPerformed -= HandleTap;
    }

    public override void Tick(float deltaTime)
    {
        // HandleHorizontalMovement(deltaTime, stateMachine.CurrentLane, 0);
    }

    private void HandleSwipeRight()
    {

    }

    private void HandleSwipeLeft()
    {

    }

    private void HandleSwipeDown()
    {

    }

    private void HandleTap(Vector2 vector)
    {
        bool isEnemyKilled = stateMachine.EnemyDetector.AttackEnemy();
        if (isEnemyKilled)
        {
            stateMachine.SwitchState(new CharacterCheckPointState(stateMachine));
        }
    }
}