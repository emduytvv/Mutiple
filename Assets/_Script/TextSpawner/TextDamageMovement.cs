using UnityEngine;

public class TextDamageMovement : Movement
{
    private Vector3 _velocity;
    private float _gravity = 1.5f;
    [SerializeField] private float _speedMin = 0.4f;
    [SerializeField] private float _speedMax = 0.7f;

    protected virtual void OnEnable()
    {
        _velocity = new Vector3(Random.Range(-_speedMin, _speedMin), Random.Range(_speedMin, _speedMax), 0);
    }

    protected override void Move()
    {
        _velocity.y -= _gravity * Time.fixedDeltaTime;
        transform.parent.position += _velocity * _moveSpeed * Time.fixedDeltaTime;
    }
}
