using UnityEngine;

public class SummonPhase2 : SummonAbility
{
    [SerializeField] private int _batOrangeCount = 10;
    [SerializeField] private int _batExplosionCount = 10;
    [SerializeField] private float _spawnInterval = 0.1f;

    protected override int BatOrangeCount => _batOrangeCount;
    protected override int BatExplosionCount => _batExplosionCount;
    protected override float SpawnInterval => _spawnInterval;
}
