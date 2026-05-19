using System;
using Photon.Pun;
using UnityEngine;

public class SlimeGreenCombat : SlimeCombat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _angleBase = 45;
        _totalBullet = 4;
        nameBullet = NameBullet.Bullet_Fire.ToString();
    }

}
