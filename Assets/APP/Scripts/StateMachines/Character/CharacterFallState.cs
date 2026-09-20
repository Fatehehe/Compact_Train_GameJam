using System;
using UnityEngine;

public class CharacterFallState : CharacterBaseState, ITap, IAnimation
{
    private readonly int FallHash = Animator.StringToHash("Fall Behind");
    private readonly int GettingUpHash = Animator.StringToHash("Getting Up");
    private int tapsRequired = 5;
    private int currentTaps;

    private float knockbackDuration;
    private float knockbackSpeed;

    private bool isFallCompleted;
    private bool isTapCompleted;

    public CharacterFallState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        currentTaps = 0;
        knockbackDuration = 0.2f;
        knockbackSpeed = 3f;
        isFallCompleted = false;
        isTapCompleted = false;

        stateMachine.Animator.CrossFadeInFixedTime(FallHash, stateMachine.Config.crossFadeDuration, 0, 0f);
    }

    public override void Tick(float deltaTime)
    {
        if (knockbackDuration > 0f)
        {
            stateMachine.transform.Translate(Vector3.back * knockbackSpeed * deltaTime);
            knockbackSpeed = Mathf.Lerp(knockbackSpeed, 0f, deltaTime * 10f);
            knockbackDuration -= deltaTime;
        }
    }

    public override void Exit() { }

    public void OnTap()
    {
        if (!isFallCompleted) return;
        if (isTapCompleted) return;

        currentTaps++;
        if (currentTaps >= tapsRequired)
        {
            isTapCompleted = true;
            stateMachine.Animator.CrossFadeInFixedTime(GettingUpHash, stateMachine.Config.crossFadeDuration, 0, 0f);
        }
    }

    public void OnFallBehindCompleted()
    {
        isFallCompleted = true;
    }

    public void OnGettingUpCompleted()
    {
        stateMachine.Target.ResetHp();
        stateMachine.SwitchState(new CharacterCheckPointState(stateMachine));
        stateMachine.isFalling = false;
    }

    public void OnFallCompleted() { }
    public void OnStandingUpCompleted() { }
}