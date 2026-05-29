using UnityEngine;

public class SummonPhase1 : SummonAbility
{
    [SerializeField] private int _batOrangeCount = 5;
    [SerializeField] private int _batExplosionCount = 5;
    [SerializeField] private float _spawnInterval = 0.2f;

    protected override int BatOrangeCount => _batOrangeCount;
    protected override int BatExplosionCount => _batExplosionCount;
    protected override float SpawnInterval => _spawnInterval;
}
