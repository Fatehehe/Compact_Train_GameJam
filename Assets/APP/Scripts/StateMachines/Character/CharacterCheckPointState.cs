using UnityEngine;

public class CharacterCheckPointState : CharacterBaseState, IAnimation, IPlayer
{
    private readonly int CheckPointHash = Animator.StringToHash("Run");

    public CharacterCheckPointState(CharacterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(CheckPointHash, stateMachine.Config.crossFadeDuration);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        MoveHorizontal(deltaTime, stateMachine.CurrentLane);
        MoveForward(deltaTime, 2f);

        // --- STATE INI MENGECEK KEDUANYA ---

        // Prioritaskan Swipeable dulu (Sesuai urutan if-else)
        if (stateMachine.EnemyDetector.IsDetectingSwipeable())
        {
            // Panggil event Pulled di State Machine (atau langsung Switch State)
            stateMachine.SwitchState(new CharacterFallState(stateMachine));
            return; // Wajib return agar logika gerak di bawahnya tidak dieksekusi lagi di frame ini
        }
        else if (stateMachine.EnemyDetector.IsDetectingPushable())
        {
            stateMachine.SwitchState(new CharacterPushingState(stateMachine));
            return;
        }
    }

    public void OnFallBehindCompleted() { }
    public void OnFallCompleted() { }
    public void OnGettingUpCompleted() { }
    public void OnStandingUpCompleted() { }
    public void OnStopAnimation() => stateMachine.SwitchState(new CharacterIdleState(stateMachine));

    public bool IsFalling => false;
    public Transform GetTransform() => stateMachine.GetTransform();

    public void OnCheckPoint() { }
    public void OnTakeDamage(float damage) { }
    public void OnKnockedOut() { }
}