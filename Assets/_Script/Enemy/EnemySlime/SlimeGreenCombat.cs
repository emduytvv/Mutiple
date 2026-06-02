using System;
using Photon.Pun;
using UnityEngine;

public class SlimeGreenCombat : SlimeCombat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _angleBase = 22;
        _totalBullet = 8;
        nameBullet = NameBullet.Bullet_Fire.ToString();
    }

}
