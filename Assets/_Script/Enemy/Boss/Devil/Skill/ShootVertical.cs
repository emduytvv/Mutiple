using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class ShootVertical : BaseBossAbility
{
    [SerializeField] private string _bulletPrefabId = NameBullet.Bullet_Devil.ToString();
    [SerializeField] private List<Transform> _points = new();

    protected abstract int PointCount { get; }
    protected abstract int BulletCount { get; }
    protected abstract float BulletInterval { get; }
    protected abstract float WarningDuration { get; }

    private readonly List<Transform> _selectedPoints = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPoints();
    }

    private void LoadPoints()
    {
        if (_points.Count > 0) return;
        foreach (Transform point in transform)
            _points.Add(point);
    }

    public override void Execute() => StartCoroutine(FireVertical());

    private void SelectPoints()
    {
        _selectedPoints.Clear();
        List<int> available = new();
        for (int i = 0; i < _points.Count; i++) available.Add(i);

        for (int i = 0; i < PointCount; i++)
        {
            int pick = Random.Range(0, available.Count);
            _selectedPoints.Add(_points[available[pick]]);
            available.RemoveAt(pick);
        }
    }

    private IEnumerator FireVertical()
    {
        SelectPoints();
        SpawnWarnings();
        yield return new WaitForSeconds(WarningDuration);

        var wait = new WaitForSeconds(BulletInterval);
        for (int i = 0; i < BulletCount; i++)
        {
            foreach (Transform point in _selectedPoints)
            {
                PhotonNetwork.Instantiate(_bulletPrefabId, point.position, Quaternion.Euler(0f, 0f, 90f));
                PhotonNetwork.Instantiate(_bulletPrefabId, point.position, Quaternion.Euler(0f, 0f, 270f));
            }
            yield return wait;
        }
    }

    private void SpawnWarnings()
    {
        foreach (Transform point in _selectedPoints)
            PhotonNetwork.Instantiate(FXName.Waring.ToString(), point.position, Quaternion.identity);
    }
}
