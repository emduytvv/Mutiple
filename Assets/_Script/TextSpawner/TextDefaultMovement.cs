using UnityEngine;

public class TextDefaultMovement : Movement
{
    protected override void Move()
    {
        transform.parent.position += Vector3.up * _moveSpeed * Time.fixedDeltaTime;
    }
}
