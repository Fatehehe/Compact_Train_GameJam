using UnityEngine;

public interface IPlayer
{
    Transform GetTransform();
    void SetPosition(Vector3 pos);
    bool IsFalling { get; }
    void OnCheckPoint();
    void OnTakeDamage(float damage);
    void OnKnockedOut();
}
