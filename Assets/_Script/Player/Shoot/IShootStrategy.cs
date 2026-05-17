using UnityEngine;

public interface IShootStrategy
{
    void Shoot(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen);
}
