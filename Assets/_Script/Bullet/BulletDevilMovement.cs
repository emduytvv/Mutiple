using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletDevilMovement : BulletMovement
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _moveSpeed = 18f;
    }
}
