using UnityEngine;

public class ShootHorizantalPhase2 : ShootHorizantal
{
    [SerializeField] private int _pointCount = 4;
    [SerializeField] private int _bulletCount = 40;
    [SerializeField] private float _bulletInterval = 0.1f;
    [SerializeField] private float _warningDuration = 2f;

    protected override int PointCount => _pointCount;
    protected override int BulletCount => _bulletCount;
    protected override float BulletInterval => _bulletInterval;
    protected override float WarningDuration => _warningDuration;
}
