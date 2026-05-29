using Photon.Pun;

public class ItemDropDespawn : Despawn
{
    protected ItemDropCtrl _itemDropCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadItemDropCtrl();
    }

    private void LoadItemDropCtrl()
    {
        if (_itemDropCtrl != null) return;
        _itemDropCtrl = GetComponentInParent<ItemDropCtrl>();
    }

    public override void DespawnObject()
    {
        ItemDropSpawner.Instance.Despawn(transform.parent);
    }

    protected override bool CanDespawn()
    {
        return false;
    }

}
