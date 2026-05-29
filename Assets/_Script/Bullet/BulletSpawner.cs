using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletSpawner : Spawner
{
    private static BulletSpawner _instance;
    public static BulletSpawner Instance => _instance;
    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 1000;
    }

    public void Spawn(string bulletName, Vector3 position, Quaternion rotation, float physDamage, float magDamage)
    {
        object[] data = { physDamage, magDamage };
        PhotonNetwork.Instantiate(bulletName, position, rotation, 0, data);
    }
}
