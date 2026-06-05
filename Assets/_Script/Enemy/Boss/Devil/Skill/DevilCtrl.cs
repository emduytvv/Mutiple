using UnityEngine;

public class DevilCtrl : BossCtrl
{
    [SerializeField] protected DevilMovement _devilMovement;
    public DevilMovement DevilMovement => _devilMovement;

    [SerializeField] protected DevilCombat _devilCombat;
    public DevilCombat DevilCombat => _devilCombat;

    [SerializeField] private Transform _childLeft;
    public Transform ChildLeft => _childLeft;

    [SerializeField] private Transform _childRight;
    public Transform ChildRight => _childRight;
    public DevilAnimation DevilAnimation => _bossAnimation as DevilAnimation;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDevilMovement();
        this.LoadDevilCombat();
        this.LoadChildLeft();
        this.LoadChildRight();
    }

    private void LoadDevilMovement()
    {
        if (_devilMovement != null) return;
        _devilMovement = GetComponentInChildren<DevilMovement>();
    }

    private void LoadDevilCombat()
    {
        if (_devilCombat != null) return;
        _devilCombat = GetComponentInChildren<DevilCombat>();
    }

    private void LoadChildLeft()
    {
        if (_childLeft != null) return;
        _childLeft = transform.Find("ChildLeft");
    }

    private void LoadChildRight()
    {
        if (_childRight != null) return;
        _childRight = transform.Find("ChildRight");
    }
}
