using UnityEngine;

public interface IPlayer
{
    Transform GetTransform();
    bool IsFalling { get; }
    void OnCheckPoint();
    void OnTakeDamage(float damage);
    void OnKnockedOut();
}
