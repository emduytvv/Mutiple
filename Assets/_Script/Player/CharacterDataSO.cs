using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "SO/Character/CharacterDataSO")]
public class CharacterDataSO : ScriptableObject
{
    [Header("HP")]
    public float baseMaxHP = 1000f;

    [Header("Defense")]
    public float basePhysicalDefense = 0f;
    public float baseMagicalDefense = 0f;

    [Header("Attack")]
    public float basePhysicalDamage = 10f;
    public float baseMagicalDamage = 0f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 6.2f;
    public int maxJumpCount = 1;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public Sprite icon;
}
