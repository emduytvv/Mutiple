using System;
using Photon.Realtime;
using UnityEngine;

public abstract class BaseInteract : SaiMonoBehaviour
{
    [SerializeField] protected Transform _iconKeyE;
    [SerializeField] protected bool _playerInRange = false;
    protected PlayerCtrl _player;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadIconKeyE();
    }
    protected void LoadIconKeyE()
    {
        if (_iconKeyE != null) return;
        _iconKeyE = transform.Find("IconKeyE");
        _iconKeyE.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load IconKeyE", gameObject);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponentInParent<PlayerCtrl>();
        if (_player == null || !_player.PhotonView.IsMine) return;
        _playerInRange = true;
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        _player = collision.GetComponentInParent<PlayerCtrl>();
        if (_player == null || !_player.PhotonView.IsMine) return;
        _playerInRange = false;
    }
    protected void Update()
    {
        ShowIconKeyE();
        Interact();
    }

    private void ShowIconKeyE()
    {
        _iconKeyE.gameObject.SetActive(_playerInRange);

    }


    protected abstract void Interact();

}
