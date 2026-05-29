using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZone : SaiMonoBehaviour
{
    [SerializeField] private WaveDataSO _waveData;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private bool _triggered = false;
    [SerializeField] private bool _wavesCleared = false;

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

    protected override void Start()
    {
        base.Start();
        GameEvents.OnAllWavesCleared += OnWavesCleared;
    }

    private void OnDestroy()
    {
        GameEvents.OnAllWavesCleared -= OnWavesCleared;
    }

    private void OnWavesCleared() => _wavesCleared = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (_wavesCleared) return;
        if (!PhotonNetwork.IsMasterClient) return;
        if (!other.GetComponent<PlayerDamageReceiver>()) return;
        _triggered = true;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int total = 0;
        foreach (SpawnEnemyProfile enemy in _waveData._enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                EnemyFactory.Instance.Create(enemy.enemyName, GetSpawnPoint(i), Quaternion.identity);
                total++;
            }
        }
        WaveManager.Instance.AddAliveCount(total);
    }

    private Vector3 GetSpawnPoint(int i)
    {
        return _spawnPoints[i % _spawnPoints.Count].position;
    }
}
