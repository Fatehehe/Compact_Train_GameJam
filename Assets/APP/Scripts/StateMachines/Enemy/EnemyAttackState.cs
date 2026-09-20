using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private const float CrossFadeDuration = 0.1f;
    private int pullDamage = 1;

    private float attackInterval = 1f; // Jeda waktu antar serangan
    private float timer; // Timer yang akan berjalan

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entering Attack State");
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);

        // Atur timer agar musuh bisa langsung menyerang atau menunggu dulu
        // Jika ingin langsung serang di detik pertama, ubah jadi timer = 0f;
        timer = attackInterval;
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        // 1. Cek terus menerus apakah target sudah mati/hilang
        if (stateMachine.Targeter.IsTargetEliminated())
        {
            // Jika sudah mati, kembali ke Idle dan HENTIKAN eksekusi di bawahnya
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        // 2. Hitung mundur timer serangan
        timer -= deltaTime;

        // 3. Jika timer habis, lakukan serangan
        if (timer <= 0f)
        {
            stateMachine.Targeter.PullTarget(pullDamage);

            // Reset timer agar dia bisa menyerang lagi di siklus berikutnya
            timer = attackInterval;

            // (Opsional) Jika kamu mau animasi serangannya di-play ulang setiap kali hit:
            // stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
        }
    }
}