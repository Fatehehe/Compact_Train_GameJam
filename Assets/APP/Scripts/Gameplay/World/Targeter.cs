using System;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private float reachDistance = 0.5f;
    public event Action OnTargetReached;
    public event Action OnTargetDetected; // Event mendeteksi target
    private bool hasReachedTarget = false;

    private List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private void Update()
    {
        if (CurrentTarget == null)
        {
            hasReachedTarget = false;
            return;
        }

        float sqrDistance = (CurrentTarget.transform.position - transform.position).sqrMagnitude;
        float sqrReachDistance = reachDistance * reachDistance;

        if (sqrDistance <= sqrReachDistance)
        {
            if (!hasReachedTarget)
            {
                hasReachedTarget = true;
                OnTargetReached?.Invoke();
            }
        }
        else
        {
            hasReachedTarget = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) { return; }
        if (targets.Contains(target)) { return; }

        targets.Add(target);
        // target.OnDestroyed += RemoveTarget;
        CurrentTarget = target;

        // (Opsional) Jika kamu ingin event ini terpanggil SETIAP KALI ada target baru 
        // yang masuk ke collider (meskipun dia bukan target terdekat), 
        // kamu bisa uncomment baris di bawah ini:
        OnTargetDetected?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Target target)) { return; }
        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Target target in targets)
        {
            if (target == null) continue;

            Vector3 directionToTarget = target.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = target;
            }
        }

        if (closestTarget == null) { return false; }

        if (CurrentTarget != closestTarget)
        {
            hasReachedTarget = false;
            OnTargetDetected?.Invoke();
        }

        CurrentTarget = closestTarget;

        return true;
    }

    public void Cancel()
    {
        CurrentTarget = null;
        hasReachedTarget = false;
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            CurrentTarget = null;
            hasReachedTarget = false;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public void PullTarget(int pullDamage)
    {
        if (CurrentTarget == null) { return; }
        CurrentTarget.Pull(pullDamage);
    }

    public bool IsTargetEliminated()
    {
        if (CurrentTarget.pullHP == 0) return true;
        return false;
    }
}