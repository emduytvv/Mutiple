using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    private static PlayerSpawner _instance;
    public static PlayerSpawner Instance => _instance;
    protected override void Awake()
    {
        base.Awake();
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    protected override void Loadprefabs()
    {
        return;
    }
}
