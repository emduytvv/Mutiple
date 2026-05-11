using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/WaveData", fileName = "WaveData")]
public class WaveDataSO : ScriptableObject
{
    public List<SpawnEnemyProfile> _enemies;
}
