using UnityEngine;

public interface IAnimation
{
    void OnFallCompleted();
    void OnFallBehindCompleted();
    void OnGettingUpCompleted();
    void OnStandingUpCompleted();
    void OnLoseAnimation();
    void OnWinAnimation();
}