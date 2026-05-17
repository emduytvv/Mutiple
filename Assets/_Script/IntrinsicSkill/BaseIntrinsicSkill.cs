
using UnityEngine;
public abstract class BaseIntrinsicSkill : SaiMonoBehaviour
{
    [SerializeField] protected bool _isActive = false;
    protected IntrinsicSkillSO _data;
    protected PlayerCtrl _player;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_player != null) return;
        _player = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }


    public void Activate(IntrinsicSkillSO so)
    {
        _data = so;
        _isActive = true;
    }
}
