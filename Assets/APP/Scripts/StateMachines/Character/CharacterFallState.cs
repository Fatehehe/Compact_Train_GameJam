using System;
using UnityEngine;

public class CharacterFallState : CharacterBaseState, ITap, IAnimation, IPlayer
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
        stateMachine.SwitchState(new CharacterCheckPointState(stateMachine));
    }

    public void OnFallCompleted() { }
    public void OnStandingUpCompleted() { }
    public void OnLoseAnimation() => stateMachine.SwitchState(new CharacterLoseState(stateMachine));
    public void OnWinAnimation() => stateMachine.SwitchState(new CharacterWinState(stateMachine));


    public bool IsFalling => true;
    public Transform GetTransform() => stateMachine.GetTransform();
    public void OnCheckPoint() { }
    public void OnTakeDamage(float damage) { }
    public void OnKnockedOut() { }
    public void SetPosition(Vector3 pos) => stateMachine.SetPosition(pos);
}