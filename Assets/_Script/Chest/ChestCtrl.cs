using UnityEngine;

public class ChestCtrl : SaiMonoBehaviour
{
    public ChestData ChestData => _chestData;
    [SerializeField] private ChestData _chestData;

    public ChestModel ChestModel => _chestModel;
    [SerializeField] private ChestModel _chestModel;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadChestData();
        LoadChestModel();
    }

    private void LoadChestData()
    {
        if (_chestData != null) return;
        _chestData = GetComponentInChildren<ChestData>();
        Debug.Log(transform.name + ": Load ChestData", gameObject);
    }

    private void LoadChestModel()
    {
        if (_chestModel != null) return;
        _chestModel = GetComponentInChildren<ChestModel>();
        Debug.Log(transform.name + ": Load ChestModel", gameObject);
    }
}
