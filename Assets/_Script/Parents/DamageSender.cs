using UnityEngine;

public class DamageSender : SaiMonoBehaviour
{
    [SerializeField] protected float basePhysicalDamage = 1f;
    [SerializeField] protected float baseMagicalDamage = 0f;
    public float PhysicalDamage => basePhysicalDamage;
    public float MagicalDamage => baseMagicalDamage;
}
