using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletMovement : Movement
{
    protected override void Move()
    {
        transform.parent.Translate(Vector3.right * (_moveSpeed * Time.fixedDeltaTime));
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        _moveSpeed = 30f;
    }
}
