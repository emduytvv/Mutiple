using UnityEngine;

public class PlayerPickup : SaiMonoBehaviour
{
    [SerializeField] private PlayerCtrl _playerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        ItemPickupable item = other.GetComponent<ItemPickupable>();
        if (item == null) return;
        item.Attract(_playerCtrl);
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (!_playerCtrl.PhotonView.IsMine) return;
    //     ItemPickupable item = other.GetComponentInParent<ItemPickupable>();
    //     if (item == null) return;
    //     item.StopAttract();
    // }
}
