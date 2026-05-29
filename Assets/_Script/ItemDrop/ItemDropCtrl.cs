using UnityEngine;

public class ItemDropCtrl : SaiMonoBehaviour
{
    public ItemDropDespawn ItemDropDespawn => _itemDropDespawn;
    [SerializeField] protected ItemDropDespawn _itemDropDespawn;

    public ItemPickupable ItemPickupable => _itemPickupable;
    [SerializeField] protected ItemPickupable _itemPickupable;

    public ItemDropMove ItemDropMove => _itemDropMove;
    [SerializeField] protected ItemDropMove _itemDropMove;

    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    [SerializeField] protected Rigidbody2D _rigidbody2D;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadItemDropDespawn();
        this.LoadItemPickupable();
        this.LoadItemDropMove();
        this.LoadRigidbody2D();
    }

    private void LoadItemDropDespawn()
    {
        if (_itemDropDespawn != null) return;
        _itemDropDespawn = GetComponentInChildren<ItemDropDespawn>();
    }

    private void LoadItemPickupable()
    {
        if (_itemPickupable != null) return;
        _itemPickupable = GetComponentInChildren<ItemPickupable>();
    }

    private void LoadItemDropMove()
    {
        if (_itemDropMove != null) return;
        _itemDropMove = GetComponentInChildren<ItemDropMove>();
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
