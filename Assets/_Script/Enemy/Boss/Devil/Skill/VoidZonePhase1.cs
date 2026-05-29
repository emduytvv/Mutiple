using UnityEngine;

public class VoidZonePhase1 : VoidZoneAbility
{
    [SerializeField] private int _circleCount = 8;
    [SerializeField] private float _interval = 1f;

    protected override int CircleCount => _circleCount;
    protected override float Interval => _interval;
}
