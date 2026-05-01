using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance => _instance;
    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }
    protected override void Start()
    {
        base.Start();
        Invoke(nameof(SpawnerTest), 3f);
    }
    protected void SpawnerTest()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.Instantiate("Enemy_1", Vector3.zero, Quaternion.identity);
    }
}
