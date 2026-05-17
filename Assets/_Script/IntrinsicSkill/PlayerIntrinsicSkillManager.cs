using System.Collections.Generic;
using UnityEngine;
public class PlayerIntrinsicSkillManager : SaiMonoBehaviour
{

    [SerializeField] private List<BaseIntrinsicSkill> _skills = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlots();
    }

    private void LoadSlots()
    {
        if (_skills.Count > 0) return;
        _skills.AddRange(GetComponentsInChildren<BaseIntrinsicSkill>(true));
        Debug.Log(transform.name + ": Load slots x" + _skills.Count, gameObject);
    }

    public void AddSkill(IntrinsicSkillSO skill)
    {
        _skills.Find(s => s.transform.name == skill._name.ToString()).Activate(skill);
    }
}
