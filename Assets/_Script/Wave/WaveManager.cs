using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : SaiMonoBehaviour
{
    private static WaveManager _instance;
    public static WaveManager Instance => _instance;

    [SerializeField] private List<WaveDataSO> _waves;
    [SerializeField] private int _currentWave = -1;
    [SerializeField] private int _aliveCount = 0;
    [SerializeField] private SpawnPointsManager _spawnPointsManager;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSpawnPointsManager();
        LoadWaveDataSO();
    }

    private void LoadWaveDataSO()
    {
        if (_waves.Count > 0) return;
        string mapFolder = "WaveData/" + SceneManager.GetActiveScene().name + "/Waves";
        _waves = Resources.LoadAll<WaveDataSO>(mapFolder).ToList();
        Debug.Log(transform.name + ": Load WaveDataSO from " + mapFolder, gameObject);
    }
    private void LoadSpawnPointsManager()
    {
        if (_spawnPointsManager != null) return;
        _spawnPointsManager = GetComponentInChildren<SpawnPointsManager>();
        Debug.Log(transform.name + ": Load SpawnPointsManager", gameObject);
    }
    protected override void Start()
    {
        base.Start();
        if (!PhotonNetwork.IsMasterClient) return;
        GameEvents.OnEnemyDied += OnEnemyDied;
        Invoke(nameof(StartFirstWave), 1f);
    }

    private void OnDestroy()
    {
        GameEvents.OnEnemyDied -= OnEnemyDied;
    }

    private void StartFirstWave() => StartWave(0);
    private void StartWave(int index)
    {
        if (index >= _waves.Count) return;
        _currentWave = index;
        _aliveCount = 0;

        foreach (SpawnEnemyProfile enemy in _waves[index]._enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                Debug.Log(enemy.count);
                EnemyFactory.Instance.Create(enemy.enemyType, GetSpawnPoint(enemy.enemyType, i), Quaternion.identity);
                _aliveCount++;
            }
        }
    }
    private void OnEnemyDied()
    {
        _aliveCount--;
        if (_aliveCount <= 0) NextWave();
    }

    private void NextWave()
    {
        int next = _currentWave + 1;
        if (next < _waves.Count)
            StartWave(next);
        else
            GameEvents.OnAllWavesCleared?.Invoke();
    }
    private Vector3 GetSpawnPoint(EnemyType type, int index)
    {
        switch (type)
        {
            case EnemyType.Bat:
                return _spawnPointsManager.Points_Bat[index % _spawnPointsManager.Points_Bat.Count].position;
            case EnemyType.MagicMini:
                return _spawnPointsManager.Points_MagicMini[index % _spawnPointsManager.Points_MagicMini.Count].position;
            case EnemyType.ShooterOnPlatform:
                return _spawnPointsManager.Points_ShooterOnPlatform[index % _spawnPointsManager.Points_ShooterOnPlatform.Count].position;
            case EnemyType.Airm:
                return _spawnPointsManager.Points_Airm[index % _spawnPointsManager.Points_Airm.Count].position;
            case EnemyType.Melee:
                return _spawnPointsManager.Points_Melee[index % _spawnPointsManager.Points_Melee.Count].position;
            case EnemyType.Slime:
                return _spawnPointsManager.Points_Slime[index % _spawnPointsManager.Points_Slime.Count].position;
        }
        Debug.LogWarning("WaveManager: không tìm thấy spawn point cho " + type);
        return Vector3.zero;
    }
}
