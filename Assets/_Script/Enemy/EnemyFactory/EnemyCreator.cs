using System;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public abstract class EnemyCreator : SaiMonoBehaviour
{
    [SerializeField] protected EnemyType _enemyType;
    public EnemyType EnemyType => _enemyType;

    [SerializeField] protected List<EnemyName> _enemyNames;
    public List<EnemyName> EnemyNames => _enemyNames;

    public virtual EnemyCtrl Create(EnemyName name, Vector3 pos, Quaternion rot, float multiplier = 1f)
    {
        if (!PhotonNetwork.IsMasterClient) return null;

        GameObject go = PhotonNetwork.Instantiate(name.ToString(), pos, rot);
        EnemyCtrl enemy = go.GetComponent<EnemyCtrl>();
        enemy.ApplyStatMultiplier(multiplier);

        SpawnHPBar(enemy);
        OnCreated(enemy);
        return enemy;
    }

    private void SpawnHPBar(EnemyCtrl enemy)
    {
        if (_enemyType == EnemyType.Boss) return;
        Transform obj = HPBarEnemySpawner.Instance.SpawnHPBar(enemy.transform.position, enemy.transform.rotation);
        obj.GetComponentInChildren<EnemyHPBar>().SetTarget(enemy.transform);
    }


    protected virtual void OnCreated(EnemyCtrl enemy) { }
}
