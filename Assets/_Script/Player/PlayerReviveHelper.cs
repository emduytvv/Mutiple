using System;
using System.Threading;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class PlayerReviveHelper : SaiMonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private float _reviveTime = 5f;
    [SerializeField] private float _timer = 0f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_photonView != null) return;
        _photonView = GetComponentInParent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_photonView.IsMine) return;
        PlayerCtrl other = collision.GetComponentInParent<PlayerCtrl>();
        if (other == null) return;
        if (!other.PlayerDamageReceiver.isDead) return;
        if (other.PhotonView == _photonView) return;

        Help(other);

    }

    private void Help(PlayerCtrl other)
    {
        if (!CanHelp()) return;
        other.PhotonView.RPC("RpcRevive", RpcTarget.All);
    }

    private bool CanHelp()
    {
        if (!InputManager.Instance.GetMouseReviveHelper())
        {
            _timer = 0f;
            return false;
        }
        if (_timer < _reviveTime)
        {
            _timer += Time.fixedDeltaTime;
            return false;
        }
        _timer = 0f;
        return true;
    }
}
