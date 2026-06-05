using UnityEngine;

public class ItemPickupable : SaiMonoBehaviour
{
    [SerializeField] private ItemDropCtrl _itemDropCtrl;
    [SerializeField] private float _pickupDistance = 0.5f;
    [SerializeField] private PlayerCtrl _targetPlayer;

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

    private void OnEnable()
    {
        _targetPlayer = null;  // reset khi spawn lại
    }
    public void Attract(PlayerCtrl player)
    {
        _targetPlayer = player;
        _itemDropCtrl.ItemDropMove.SetTarget(player.transform);
        _itemDropCtrl.Rigidbody2D.simulated = false;
    }

    public void StopAttract()
    {
        _targetPlayer = null;
        _itemDropCtrl.ItemDropMove.SetTarget(null);
    }

    private void FixedUpdate()
    {
        this.CheckPickup();
    }

    private void CheckPickup()
    {
        if (_targetPlayer == null) return;
        float dist = Vector2.Distance(transform.position, _targetPlayer.transform.position + Vector3.up * 0.5f);
        if (dist > _pickupDistance) return;
        this.Pickup();
    }

    private void Pickup()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.GoldPickupSFX);
        _targetPlayer.GetComponentInChildren<PlayerGold>().AddGold(20);
        _itemDropCtrl.ItemDropDespawn.DespawnObject();
    }
}
