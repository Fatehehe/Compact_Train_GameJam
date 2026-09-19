using UnityEngine;

public class CharacterCheckPointState : CharacterBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Idle");
    // private float duration = .5f;
    // private float currentKnockbackSpeed;

    public CharacterCheckPointState(CharacterStateMachine stateMachine) : base(stateMachine)
    {
        // this.currentKnockbackSpeed = knockBack;
    }

    public override void Enter()
    {
        Debug.Log("Enter Check Point State");
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // stateMachine.transform.Translate(Vector3.back * currentKnockbackSpeed * deltaTime);
        // currentKnockbackSpeed = Mathf.Lerp(currentKnockbackSpeed, 0f, deltaTime * 5f);

        // duration -= deltaTime;
        // if (duration <= 0f)
        // {
        //     stateMachine.SwitchState(new CharacterMovingState(stateMachine));
        // }
    }

    public override void Exit()
    {

    }
}
