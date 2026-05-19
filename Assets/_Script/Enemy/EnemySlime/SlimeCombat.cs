using System;
using Photon.Pun;
using UnityEngine;

public class SlimeCombat : EnemyCombat<SlimeCtrl>
{
    protected string nameBullet = NameBullet.Bullet_Fire.ToString();
    protected int _angle = 90;
    protected int _angleBase = 45;
    protected int _totalBullet = 4;
    public virtual void Implement()
    {
        for (int i = 0; i < _totalBullet; i++)
        {
            int z = _angleBase + _angle * i;
            Quaternion rot = Quaternion.Euler(0, 0, z);
            SpawnBullet(rot);
        }
    }

    protected virtual void SpawnBullet(Quaternion quaternion)
    {
        PhotonNetwork.Instantiate(nameBullet, transform.parent.position, quaternion);
    }

}
