using UnityEngine;

public class SpikeBurstPhase1 : SpikeBurstAbility
{
    [SerializeField] private int _bulletCount = 36;
    protected override int BulletCount => _bulletCount;
}
