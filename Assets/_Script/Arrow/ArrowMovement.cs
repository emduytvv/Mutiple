using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowMovement : Movement
{
    protected override void Move()
    {
        transform.parent.Translate(Vector3.right * (_moveSpeed * Time.fixedDeltaTime));
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        _moveSpeed = 50f;
    }
}
