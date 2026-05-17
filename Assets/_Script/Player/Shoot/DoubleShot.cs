using System.Collections;
using Photon.Pun;
using UnityEngine;

public class DoubleShot : IShootStrategy
{
    private const float SpawnRadius = 0.5f;
    private readonly MonoBehaviour _runner;
    private readonly float _delay = 0.15f;

    public DoubleShot(MonoBehaviour runner)
    {
        _runner = runner;
    }

    public void Shoot(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen)
    {
        _runner.StartCoroutine(ShootTwice(prefabName, spawnCenter, angle180, phys, mag, pen));
    }

    private IEnumerator ShootTwice(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen)
    {
        SpawnArrow(prefabName, spawnCenter, angle180, phys, mag, pen);
        yield return new WaitForSeconds(_delay);
        SpawnArrow(prefabName, spawnCenter, angle180, phys, mag, pen);
    }

    private void SpawnArrow(string prefabName, Vector3 spawnCenter, float angle180, float phys, float mag, float pen)
    {
        float rad = angle180 * Mathf.Deg2Rad;
        Vector3 pos = spawnCenter + new Vector3(Mathf.Cos(rad) * SpawnRadius, Mathf.Sin(rad) * SpawnRadius, 0);
        GameObject arrow = PhotonNetwork.Instantiate(prefabName, pos, Quaternion.Euler(0, 0, angle180));
        arrow.GetComponentInChildren<ArrowDamageSender>().SetDamage(phys, mag, pen);
    }
}
