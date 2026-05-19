using System;
using Photon.Pun;
using UnityEngine;

public class SlimeCarrotCombat : SlimeCombat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _angleBase = 0;
        _totalBullet = 4;
        nameBullet = NameBullet.Bullet_Fire.ToString();
    }

}
