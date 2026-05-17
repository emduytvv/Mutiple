using UnityEngine;

public class TextDefaultMovement : Movement
{
    protected override void Move()
    {
        transform.parent.position += Vector3.up * maxSpeed * Time.fixedDeltaTime;
    }
}
