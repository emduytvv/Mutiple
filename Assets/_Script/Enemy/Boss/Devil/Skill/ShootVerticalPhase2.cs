using UnityEngine;

public class ShootVerticalPhase2 : ShootVertical
{
    [SerializeField] private int _pointCount = 55;
    [SerializeField] private int _bulletCount = 12;
    [SerializeField] private float _bulletInterval = 0.6f;
    [SerializeField] private float _warningDuration = 2f;

    protected override int PointCount => _pointCount;
    protected override int BulletCount => _bulletCount;
    protected override float BulletInterval => _bulletInterval;
    protected override float WarningDuration => _warningDuration;
}
