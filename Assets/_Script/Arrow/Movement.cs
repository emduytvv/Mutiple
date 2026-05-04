using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class Movement : SaiMonoBehaviour
{
    [SerializeField] protected float maxSpeed = 2f;
    [SerializeField] protected float baseMaxSpeed = 2f;
    protected void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

}
