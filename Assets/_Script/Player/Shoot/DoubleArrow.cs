using Photon.Pun;
using UnityEngine;

public class DoubleArrow : IShootStrategy
{
    private const float SpawnRadius = 0.5f;
    private const float SideOffset = 0.2f;

    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen)
    {
        float rad = angle180 * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
        Vector3 perp = new Vector3(-dir.y, dir.x, 0);

        Quaternion rot = Quaternion.Euler(0, 0, angle180);
        GameObject a1 = PhotonNetwork.Instantiate(prefabName, spawnCenter + dir * SpawnRadius + perp * SideOffset, rot);
        GameObject a2 = PhotonNetwork.Instantiate(prefabName, spawnCenter + dir * SpawnRadius - perp * SideOffset, rot);
        a1.GetComponentInChildren<ArrowDamageSender>().SetDamage(phys, mag, pen);
        a2.GetComponentInChildren<ArrowDamageSender>().SetDamage(phys, mag, pen);
    }
}
