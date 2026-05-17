using Photon.Pun;
using UnityEngine;

public class SpreadThreeShot : IShootStrategy
{
    private const float SpawnRadius = 0.5f;
    private readonly int _count = 3;
    private readonly float _spreadDeg = 15;
    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen)
    {
        float startAngle = angle180 - _spreadDeg * (_count - 1) / 2f;
        for (int i = 0; i < _count; i++)
        {
            float angle = startAngle + _spreadDeg * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 pos = spawnCenter + new Vector3(Mathf.Cos(rad) * SpawnRadius, Mathf.Sin(rad) * SpawnRadius, 0);
            GameObject arrow = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(0, 0, angle));
            arrow.GetComponentInChildren<ArrowDamageSender>().SetDamage(phys, mag, pen);
        }
    }
}
