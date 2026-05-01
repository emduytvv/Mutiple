using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowSpawner : Spawner
{
    private static ArrowSpawner _instance;
    public static ArrowSpawner Instance => _instance;
    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 100;
    }
    // protected override void Start()
    // {
    //     base.Start();
    //     Invoke(nameof(SpawnerTest), 3f);
    // }
    // protected void SpawnerTest()
    // {
    //     if (!PhotonNetwork.IsMasterClient) return;
    //     PhotonNetwork.Instantiate("Arrow_Raidon", Vector3.zero, Quaternion.identity);
    // }

}
