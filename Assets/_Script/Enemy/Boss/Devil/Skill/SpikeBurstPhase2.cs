using UnityEngine;

public class SpikeBurstPhase2 : SpikeBurstAbility
{
    [SerializeField] private int _bulletCount = 72;
    protected override int BulletCount => _bulletCount;
}
