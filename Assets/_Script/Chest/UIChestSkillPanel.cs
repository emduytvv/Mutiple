using System.Collections.Generic;
using UnityEngine;

// Để GameObject này INACTIVE trong scene — tự bật khi Show() được gọi
public class UIChestSkillPanel : SaiMonoBehaviour
{
    [SerializeField] private List<UIChestSkillSlot> _slots = new();

    private PlayerCtrl _pendingPlayer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlots();
    }

    private void LoadSlots()
    {
        if (_slots.Count > 0) return;
        _slots.AddRange(GetComponentsInChildren<UIChestSkillSlot>(true));
        Debug.Log(transform.name + ": Load UIChestSkillSlot x" + _slots.Count, gameObject);
    }

    public void Show(List<IntrinsicSkillSO> skills, PlayerCtrl player)
    {
        _pendingPlayer = player;
        gameObject.SetActive(true);

        for (int i = 0; i < _slots.Count; i++)
        {
            bool hasSkill = i < skills.Count;
            _slots[i].gameObject.SetActive(hasSkill);
            if (hasSkill) _slots[i].SetSkill(skills[i], this);
        }
    }

    public void OnSlotClicked(IntrinsicSkillSO skill)
    {
        if (_pendingPlayer == null) return;
        _pendingPlayer.GetComponentInChildren<PlayerIntrinsicSkillManager>().AddSkill(skill);
        Hide();
    }
    public void Hide()
    {
        _pendingPlayer = null;//.AddSkill(skill);
        gameObject.SetActive(false);
    }
}
