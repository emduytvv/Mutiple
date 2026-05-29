using UnityEngine;

public class ShootHorizantalPhase1 : ShootHorizantal
{
    [SerializeField] private int _pointCount = 3;
    [SerializeField] private int _bulletCount = 30;
    [SerializeField] private float _bulletInterval = 0.2f;
    [SerializeField] private float _warningDuration = 4f;

    protected override int PointCount => _pointCount;
    protected override int BulletCount => _bulletCount;
    protected override float BulletInterval => _bulletInterval;
    protected override float WarningDuration => _warningDuration;
}
