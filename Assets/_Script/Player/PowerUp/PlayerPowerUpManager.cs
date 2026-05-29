using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpManager : SaiMonoBehaviour
{
    [SerializeField] private PlayerCtrl _playerCtrl;
    [SerializeField] private List<BasePowerUpEffect> _effects = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerCtrl();
        LoadEffects();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    private void LoadEffects()
    {
        if (_effects.Count > 0) return;
        _effects.AddRange(GetComponentsInChildren<BasePowerUpEffect>(true));
        Debug.Log(transform.name + ": Load Effects x" + _effects.Count, gameObject);
    }

    public void Use(int inventoryIndex)
    {
        if (!_playerCtrl.PhotonView.IsMine) return;
        var items = _playerCtrl.InventoryManager.Items;

        var item = items[inventoryIndex];
        if (item?._info is not PowerUpDataSO so) return;
        BasePowerUpEffect effect = _effects.Find(e => e.EffectName == so._effectName);

        effect.Activate(so);
        ConsumeItem(item, inventoryIndex);
    }

    private void ConsumeItem(ItemInventoryBase item, int index)
    {
        if (item._amount > 1)
            item._amount--;
        else
            _playerCtrl.InventoryManager.Remove(index);
    }
}
