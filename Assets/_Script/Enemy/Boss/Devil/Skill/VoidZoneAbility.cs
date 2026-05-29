using System.Collections;
using Photon.Pun;
using UnityEngine;

public abstract class VoidZoneAbility : BaseBossAbility
{
    [SerializeField] protected string _nameSkillBoss = NameSkillBoss.DevilExplosion.ToString();

    protected abstract int CircleCount { get; }
    protected abstract float Interval { get; }

    public override void Execute() => StartCoroutine(FireVolley());

    private IEnumerator FireVolley()
    {
        if (!SetTargets()) yield break;
        var wait = new WaitForSeconds(Interval);
        for (int i = 0; i < CircleCount; i++)
        {
            SpawnZoneCircle(_player1.transform.position);
            SpawnZoneCircle(_player2.transform.position);
            yield return wait;
        }
    }

    private void SpawnZoneCircle(Vector3 spawnPoint)
    {
        PhotonNetwork.Instantiate(_nameSkillBoss, spawnPoint, Quaternion.identity);
    }
}
