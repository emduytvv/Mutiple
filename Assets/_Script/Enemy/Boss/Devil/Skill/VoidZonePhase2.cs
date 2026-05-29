using UnityEngine;

public class VoidZonePhase2 : VoidZoneAbility
{
    [SerializeField] private int _circleCount = 15;
    [SerializeField] private float _interval = 0.5f;

    protected override int CircleCount => _circleCount;
    protected override float Interval => _interval;
}
