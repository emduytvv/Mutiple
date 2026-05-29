using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "SO/Enemy/EnemyStatsSO")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("HP")]
    public float _baseMaxHP = 100f;

    [Header("Defense")]
    public float _physicalDefense = 0f;
    public float _magicalDefense = 0f;

    [Header("Attack")]
    public float _physicalAttack = 0f;
    public float _magicalAttack = 0f;
    public float _moveSpeed = 0f;
    public float _cooldown = 0f;
    [Header("Item Drop")]
    public List<TableItemDrop> itemDrops;
}
