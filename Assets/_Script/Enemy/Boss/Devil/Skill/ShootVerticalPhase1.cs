using UnityEngine;

public class ShootVerticalPhase1 : ShootVertical
{
    [SerializeField] private int _pointCount = 50;
    [SerializeField] private int _bulletCount = 7;
    [SerializeField] private float _bulletInterval = 0.6f;
    [SerializeField] private float _warningDuration = 4f;

    protected override int PointCount => _pointCount;
    protected override int BulletCount => _bulletCount;
    protected override float BulletInterval => _bulletInterval;
    protected override float WarningDuration => _warningDuration;
}
