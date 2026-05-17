using System.Collections.Generic;
using UnityEngine;

public class ChestData : SaiMonoBehaviour
{
    private const int OfferedCount = 3;
    private const string _path = "IntrinsicSkill";

    [SerializeField] private List<IntrinsicSkillSO> _allSkills = new();
    [SerializeField] private List<IntrinsicSkillSO> _currentSkills = new();

    public List<IntrinsicSkillSO> OfferedSkills => _currentSkills;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAllSkills();
    }

    private void LoadAllSkills()
    {
        if (_allSkills.Count > 0) return;
        _allSkills.AddRange(Resources.LoadAll<IntrinsicSkillSO>(_path));
        Debug.Log(transform.name + ": Load " + _allSkills.Count + " IntrinsicSkillSO from " + _path, gameObject);
    }

    protected override void Start()
    {
        base.Start();
        PickRandomSkills();
    }

    private void PickRandomSkills()
    {
        if (_allSkills.Count == 0) return;

        var pool = new List<IntrinsicSkillSO>(_allSkills);
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        _currentSkills.Clear();
        int count = Mathf.Min(OfferedCount, pool.Count);
        for (int i = 0; i < count; i++)
            _currentSkills.Add(pool[i]);
    }
}
