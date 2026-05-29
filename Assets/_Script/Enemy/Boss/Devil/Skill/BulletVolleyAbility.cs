using System.Collections;
using Photon.Pun;
using UnityEngine;

public abstract class BulletVolleyAbility : BaseBossAbility
{
    [SerializeField] protected string _bulletPrefabId = NameBullet.Bullet_Devil.ToString();

    protected abstract int BulletCount { get; }
    protected abstract float BulletInterval { get; }

    public override void Execute() => StartCoroutine(FireVolley());

    private IEnumerator FireVolley()
    {
        if (!SetTargets()) yield break;
        var wait = new WaitForSeconds(BulletInterval);
        for (int i = 0; i < BulletCount; i++)
        {
            SpawnBullet(_devilCtrl.ChildLeft, _player1);
            SpawnBullet(_devilCtrl.ChildRight, _player2);
            yield return wait;
        }
    }

    private void SpawnBullet(Transform spawnPoint, PlayerCtrl target)
    {
        Vector3 dir = (target.transform.position - spawnPoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        PhotonNetwork.Instantiate(_bulletPrefabId, spawnPoint.position, Quaternion.Euler(0f, 0f, angle));
    }
}
