using UnityEngine;

public class PlayerItemTransfer : SaiMonoBehaviour
{
    [SerializeField] private PlayerCtrl _playerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (_playerCtrl != null) return;
        _playerCtrl = GetComponentInParent<PlayerCtrl>();
        Debug.Log(transform.name + ": Load PlayerCtrl", gameObject);
    }

    public void ReceiveItem(string json)
    {
        ItemInventoryBase item = ItemTransferData.Deserialize(json);
        if (item == null) return;

        bool added = _playerCtrl.InventoryManager.AddItem(item);
        if (!added)
        {
            if (_playerCtrl.PhotonView.IsMine)
                SpawnText("Inventory full!");
            return;
        }

        GameEvents.OnItemReceived?.Invoke();
    }

    private void SpawnText(string msg)
    {
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Transform obj = TextSpawner.Instance.SpawnTextDefault(center);
        obj.GetComponent<TextDefaultCtrl>().SetText(msg);
    }
}
