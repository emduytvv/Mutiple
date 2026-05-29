using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class ShootHorizantal : BaseBossAbility
{
    [SerializeField] private string _bulletPrefabId = NameBullet.Bullet_Devil.ToString();
    [SerializeField] private Transform[] _pointsLeft = new Transform[5];
    [SerializeField] private Transform[] _pointsRight = new Transform[5];

    protected abstract int PointCount { get; }
    protected abstract int BulletCount { get; }
    protected abstract float BulletInterval { get; }
    protected abstract float WarningDuration { get; }

    private readonly List<Transform> _selectedLeft = new();
    private readonly List<Transform> _selectedRight = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPoints();
    }

    private void LoadPoints()
    {
        for (int i = 0; i < 5; i++)
        {
            if (_pointsLeft[i] == null)
                _pointsLeft[i] = transform.Find($"PointLeft_{i + 1}");
            if (_pointsRight[i] == null)
                _pointsRight[i] = transform.Find($"PointRight_{i + 1}");
        }
    }

    public override void Execute() => StartCoroutine(FireWall());

    private void SelectPoints()
    {
        _selectedLeft.Clear();
        _selectedRight.Clear();
        List<int> available = new() { 0, 1, 2, 3, 4 };
        for (int i = 0; i < PointCount; i++)
        {
            int pick = Random.Range(0, available.Count);
            int idx = available[pick];
            available.RemoveAt(pick);
            _selectedLeft.Add(_pointsLeft[idx]);
            _selectedRight.Add(_pointsRight[idx]);
        }
    }

    private IEnumerator FireWall()
    {
        SelectPoints();
        SpawnWarnings();
        yield return new WaitForSeconds(WarningDuration);

        var wait = new WaitForSeconds(BulletInterval);
        for (int i = 0; i < BulletCount; i++)
        {
            foreach (Transform point in _selectedLeft)
                PhotonNetwork.Instantiate(_bulletPrefabId, point.position, Quaternion.identity);
            foreach (Transform point in _selectedRight)
                PhotonNetwork.Instantiate(_bulletPrefabId, point.position, Quaternion.Euler(0f, 0f, 180f));
            yield return wait;
        }
    }

    private void SpawnWarnings()
    {
        foreach (Transform point in _selectedLeft)
            PhotonNetwork.Instantiate(FXName.Waring.ToString(), point.position, Quaternion.identity);
        foreach (Transform point in _selectedRight)
            PhotonNetwork.Instantiate(FXName.Waring.ToString(), point.position, Quaternion.identity);
    }
}
