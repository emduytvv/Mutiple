using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SummonAbility : BaseBossAbility
{
    [SerializeField] protected List<Transform> _points = new();

    protected abstract int BatOrangeCount { get; }
    protected abstract int BatExplosionCount { get; }
    protected abstract float SpawnInterval { get; }

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

    public override void Execute() => StartCoroutine(Summon());

    private IEnumerator Summon()
    {
        var wait = new WaitForSeconds(SpawnInterval);
        int totalCount = BatOrangeCount + BatExplosionCount;
        List<Transform> selected = SelectPoints(totalCount);

        for (int i = 0; i < BatOrangeCount; i++)
        {
            EnemyFactory.Instance.Create(EnemyName.BatPink, selected[i].position, Quaternion.identity);
            yield return wait;
        }

        for (int i = 0; i < BatExplosionCount; i++)
        {
            EnemyFactory.Instance.Create(EnemyName.BatExplosion, selected[i].position, Quaternion.identity);
            yield return wait;
        }
    }

    private List<Transform> SelectPoints(int count)
    {
        List<Transform> result = new();
        List<int> available = new();
        for (int i = 0; i < _points.Count; i++) available.Add(i);

        int pickCount = Mathf.Min(count, available.Count);
        for (int i = 0; i < pickCount; i++)
        {
            int pick = Random.Range(0, available.Count);
            result.Add(_points[available[pick]]);
            available.RemoveAt(pick);
        }

        while (result.Count < count)
            result.Add(_points[Random.Range(0, _points.Count)]);

        return result;
    }
}
