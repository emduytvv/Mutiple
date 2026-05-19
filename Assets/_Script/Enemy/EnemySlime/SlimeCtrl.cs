using UnityEngine;

public class SlimeCtrl : EnemyCtrl
{
    public SlimeCombat SlimeCombat => _slimeCombat;
    [SerializeField] private SlimeCombat _slimeCombat;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSlimeCombat();
    }

    private void LoadSlimeCombat()
    {
        if (_slimeCombat != null) return;
        _slimeCombat = GetComponentInChildren<SlimeCombat>();
        Debug.Log(transform.name + ": Load SlimeCombat", gameObject);
    }
}
