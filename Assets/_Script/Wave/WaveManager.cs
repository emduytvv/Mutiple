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
    [SerializeField] private bool _allWavesCleared = false;
    [SerializeField] private SpawnPointsManager _spawnPointsManager;
    [SerializeField] private bool test = false;
    protected void Update()
    {
        if (!test) return;
        test = false;
        GameEvents.OnAllWavesCleared?.Invoke();
    }
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
    }
    private void LoadSpawnPointsManager()
    {
        if (_spawnPointsManager != null) return;
        _spawnPointsManager = GetComponentInChildren<SpawnPointsManager>();
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

        var spawnIndex = new Dictionary<EnemyType, int>();
        foreach (SpawnEnemyProfile profile in _waves[index]._enemies)
        {
            EnemyType group = EnemyFactory.Instance.GetEnemyType(profile.enemyName);
            if (!spawnIndex.ContainsKey(group)) spawnIndex[group] = 0;
            for (int i = 0; i < profile.count; i++)
            {
                EnemyFactory.Instance.Create(profile.enemyName, GetSpawnPoint(group, spawnIndex[group]), Quaternion.identity, GameManager.Instance.StatMultiplier);
                spawnIndex[group]++;
                _aliveCount++;
            }
        }
    }
    public void AddAliveCount(int count)
    {
        if (_allWavesCleared) return;
        _aliveCount += count;
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
        {
            _allWavesCleared = true;
            GameEvents.OnAllWavesCleared?.Invoke();
        }
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
            case EnemyType.Explosion:
                return _spawnPointsManager.Points_Explosion[index % _spawnPointsManager.Points_Explosion.Count].position;
        }
        Debug.LogWarning("WaveManager: khÃ´ng tÃ¬m tháº¥y spawn point cho " + type);
        return Vector3.zero;
    }
}
