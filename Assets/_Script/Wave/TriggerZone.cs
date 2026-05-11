using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZone : SaiMonoBehaviour
{
    [SerializeField] private WaveDataSO _waveData;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private bool _triggered = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSpawnPoints();
        LoadWaveDataSO();
    }
    private void LoadWaveDataSO()
    {
        if (_waveData != null) return;
        string path = "WaveData/" + SceneManager.GetActiveScene().name + "/TriggerZone/" + transform.name;
        _waveData = Resources.Load<WaveDataSO>(path);
        Debug.Log(transform.name + ":LoadWaveDataSO " + path, gameObject);
    }

    private void LoadSpawnPoints()
    {
        if (_spawnPoints.Count > 0) return;
        foreach (Transform point in transform)
            _spawnPoints.Add(point);
        Debug.Log(transform.name + ": Load SpawnPoints", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (!PhotonNetwork.IsMasterClient) return;
        if (!other.GetComponent<PlayerDamageReceiver>()) return;
        _triggered = true;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach (SpawnEnemyProfile enemy in _waveData._enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                EnemyFactory.Instance.Create(enemy.enemyType, GetSpawnPoint(enemy.enemyType, i), Quaternion.identity);
            }
        }
    }

    private Vector3 GetSpawnPoint(EnemyType enemyType, int i)
    {
        return _spawnPoints[i % _spawnPoints.Count].position;
    }

}
