using Photon.Pun;
using UnityEngine;

public class MagicMiniCombat : EnemyShooterCombat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _bulletName = NameBullet.Bullet_Fire.ToString();
    }

}
