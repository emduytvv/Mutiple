using Photon.Pun;
using UnityEngine;

public abstract class SpikeBurstAbility : BaseBossAbility
{
    [SerializeField] protected string _bulletPrefabId = NameBullet.Bullet_Devil.ToString();

    protected abstract int BulletCount { get; }

    public override void Execute()
    {
        FireBurst(_devilCtrl.ChildLeft);
        FireBurst(_devilCtrl.ChildRight);
    }

    private void FireBurst(Transform spawnPoint)
    {
        float angleStep = 360f / BulletCount;
        for (int i = 0; i < BulletCount; i++)
        {
            float angle = i * angleStep;
            PhotonNetwork.Instantiate(_bulletPrefabId, spawnPoint.position, Quaternion.Euler(0f, 0f, angle));
        }
    }
}
