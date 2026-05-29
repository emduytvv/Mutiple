using UnityEngine;

public class DevilExplosionAnimation : SaiMonoBehaviour
{
    [SerializeField] private DevilExplosionCtrl _ctrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCtrl();
    }

    private void LoadCtrl()
    {
        if (_ctrl != null) return;
        _ctrl = GetComponentInParent<DevilExplosionCtrl>();
    }
    public void SendByEvent()
    {
        _ctrl.DamageSender.Send();
    }
}
