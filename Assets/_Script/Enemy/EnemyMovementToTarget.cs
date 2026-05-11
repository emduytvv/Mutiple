using Photon.Pun;
using UnityEngine;

public class EnemyMovementToTarget : EnemyMovement
{
    [SerializeField] protected Transform _target;
    public Transform Target => _target;
    [SerializeField] protected Vector2 _direction;
    public Vector2 Direction => _direction;
    [SerializeField] protected float _currentDistance;
    [SerializeField] protected float _minDistanceToStop = 1f;
    public void SetTarget(Transform target)
    {
        this._target = target;
    }
}
