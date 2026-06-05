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
    }

    public void AddSkill(IntrinsicSkillSO skill)
    {
        var found = _skills.Find(s => s.transform.name == skill._name.ToString());
        if (found == null)
        {
            Debug.LogWarning($"{transform.name}: Skill slot '{skill._name}' not found", gameObject);
            return;
        }
        found.Activate(skill);
    }
}
