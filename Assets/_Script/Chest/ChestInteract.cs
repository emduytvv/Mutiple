using System;
using UnityEngine;

public class ChestInteract : BaseInteract
{
    [SerializeField] private ChestCtrl _chestCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCtrl();
    }

    private void LoadCtrl()
    {
        if (_chestCtrl != null) return;
        _chestCtrl = GetComponentInParent<ChestCtrl>();
        Debug.Log(transform.name + ": Load ChestCtrl", gameObject);
    }


    private bool _isOpened = false;
    protected override void Interact()
    {
        if (!_playerInRange) return;
        if (InputManager.Instance.GetMouseIteract())
        {
            TryOpen();
        }
    }
    public void TryOpen()
    {
        if (_isOpened) return;
        _isOpened = true;
        _chestCtrl.ChestModel.PlayOpen();
        CenterCtrl.Instance.OpenChestSkillPanel(_chestCtrl, _player);
    }

}
