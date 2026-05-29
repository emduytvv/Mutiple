using UnityEngine;

public class BulletVolleyPhase2 : BulletVolleyAbility
{
    [SerializeField] private int _bulletCount = 12;
    [SerializeField] private float _bulletInterval = 0.08f;

    protected override int BulletCount => _bulletCount;
    protected override float BulletInterval => _bulletInterval;
}
