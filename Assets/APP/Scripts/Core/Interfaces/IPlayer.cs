using UnityEngine;

public interface IPlayer
{
    Transform GetTransform();
    void OnCheckPoint();
    void OnTakeDamage(float damage);
    void OnKnockedOut();
}
