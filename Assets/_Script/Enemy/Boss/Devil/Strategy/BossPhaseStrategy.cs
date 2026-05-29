using System.Collections.Generic;
using UnityEngine;

public abstract class BossPhaseStrategy : SaiMonoBehaviour, IBossPhaseStrategy
{
    [SerializeField] protected List<BaseBossAbility> _skills = new();
    private int _lastIndex = -1;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSkills();
    }

    private void LoadSkills()
    {
        if (_skills.Count > 0) return;
        foreach (Transform skill in transform)
            _skills.Add(skill.GetComponentInChildren<BaseBossAbility>());
    }


    public void Execute()
    {
        if (_skills.Count == 0) return;
        int index;
        do { index = Random.Range(0, _skills.Count); }
        while (_skills.Count > 1 && index == _lastIndex);
        _lastIndex = index;
        _skills[index].Execute();
    }
}
