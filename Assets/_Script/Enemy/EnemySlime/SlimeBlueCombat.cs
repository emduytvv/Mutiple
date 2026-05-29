using Photon.Pun;
using UnityEngine;

public class SlimeBlueCombat : SlimeCombat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        _totalBullet = 8;
        nameBullet = NameBullet.Bullet_Ice.ToString();
    }
    public override void Implement()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        for (int i = 0; i < _totalBullet; i++)
        {
            _angle = Random.Range(0, 60);
            _angleBase = Random.Range(0, 90);
            int z = _angleBase + _angle * i;
            Quaternion rot = Quaternion.Euler(0, 0, z);
            SpawnBullet(rot);
        }
    }
}
