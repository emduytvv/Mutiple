using Photon.Pun;
using UnityEngine;

public class SingleShot : IShootStrategy
{
    private const float SpawnRadius = 0.5f;

    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen)
    {
        float rad = angle180 * Mathf.Deg2Rad;
        Vector3 pos = spawnCenter + new Vector3(Mathf.Cos(rad) * SpawnRadius, Mathf.Sin(rad) * SpawnRadius, 0);
        GameObject arrow = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(0, 0, angle180));
        arrow.GetComponentInChildren<ArrowDamageSender>().SetDamage(phys, mag, pen);
    }
}
