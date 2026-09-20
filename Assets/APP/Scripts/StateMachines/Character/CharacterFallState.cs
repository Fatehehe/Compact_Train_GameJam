using UnityEngine;

public class CharacterFallState : CharacterBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");
    private int tapsRequired = 5;
    private int currentTaps;

    private float knockbackDuration;
    private float knockbackSpeed;

    public CharacterFallState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("character state: CharacterFallState");

        currentTaps = 0;
        knockbackDuration = 0.2f;
        knockbackSpeed = 3f;
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, stateMachine.Config.crossFadeDuration, 0, 0f);

        PlayerEvents.OnTapPerformed += HandleTap;
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

    public override void Exit()
    {
        PlayerEvents.OnTapPerformed -= HandleTap;
    }

    private void HandleTap(Vector2 tapPosition)
    {
        currentTaps++;
        if (currentTaps >= tapsRequired)
        {
            Debug.Log("Karakter berhasil bangun!");
            stateMachine.Target.ResetHp();
            stateMachine.SwitchState(new CharacterCheckPointState(stateMachine));
            stateMachine.isFalling = false;
        }
    }
}