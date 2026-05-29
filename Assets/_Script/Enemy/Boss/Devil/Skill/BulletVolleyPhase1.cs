using UnityEngine;

public class BulletVolleyPhase1 : BulletVolleyAbility
{
    [SerializeField] private int _bulletCount = 12;
    [SerializeField] private float _bulletInterval = 0.15f;

    protected override int BulletCount => _bulletCount;
    protected override float BulletInterval => _bulletInterval;
}
