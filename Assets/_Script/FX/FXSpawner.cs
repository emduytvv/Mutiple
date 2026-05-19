using System;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class FXSpawner : Spawner
{
    private static FXSpawner _instance;
    public static FXSpawner Instance => _instance;
    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 50;
    }
    // public void SpawnImpactArrow(Vector3 pos, quaternion rot)
    // {
    //     PhotonNetwork.Instantiate(FXName.ImpactArrow.ToString(), pos, Quaternion.identity);
    // }
}
