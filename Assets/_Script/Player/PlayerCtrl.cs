using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : SaiMonoBehaviour
{
    public PhotonView PhotonView => _photonView;
    [SerializeField] protected PhotonView _photonView;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    public PlayerMovement PlayerMovement => _playerMovement;
    [SerializeField] protected PlayerMovement _playerMovement;
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    [SerializeField] protected PlayerAnimation _playerAnimation;
    public PlayerDamageReceiver PlayerDamageReceiver => _playerDamageReceiver;
    [SerializeField] protected PlayerDamageReceiver _playerDamageReceiver;
    public PlayerDespawn PlayerDespawn => _playerDespawn;
    [SerializeField] protected PlayerDespawn _playerDespawn;
    public TextMeshPro TextMeshPro => _textMeshPro;
    [SerializeField] protected TextMeshPro _textMeshPro;

    private static List<PlayerCtrl> _allPlayers = new();
    public static List<PlayerCtrl> AllPlayers => _allPlayers;
    public string photonNickName = "offline";

    protected override void Awake()
    {
        base.Awake();
        _allPlayers.Add(this);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPhotonView();
        this.LoadRigidbody2D();
        this.LoadPlayerMovement();
        this.LoadPlayerAnimation();
        this.LoadPlayerDamageReceiver();
        this.LoadPlayerDespawn();
        this.LoadTextMeshPro();
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
        Debug.Log(transform.name + ": Load PhotonView", gameObject);
    }

    private void LoadRigidbody2D()
    {
        if (_rigidbody2D != null) return;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Debug.Log(transform.name + ": Load Rigidbody2D", gameObject);
    }

    private void LoadPlayerMovement()
    {
        if (_playerMovement != null) return;
        _playerMovement = GetComponentInChildren<PlayerMovement>();
        Debug.Log(transform.name + ": Load PlayerMovement", gameObject);
    }

    private void LoadPlayerAnimation()
    {
        if (_playerAnimation != null) return;
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        Debug.Log(transform.name + ": Load PlayerAnimation", gameObject);
    }

    private void LoadPlayerDamageReceiver()
    {
        if (_playerDamageReceiver != null) return;
        _playerDamageReceiver = GetComponentInChildren<PlayerDamageReceiver>();
        Debug.Log(transform.name + ": Load PlayerDamageReceiver", gameObject);
    }

    private void LoadPlayerDespawn()
    {
        if (_playerDespawn != null) return;
        _playerDespawn = GetComponentInChildren<PlayerDespawn>();
        Debug.Log(transform.name + ": Load PlayerDespawn", gameObject);
    }

    private void LoadTextMeshPro()
    {
        if (_textMeshPro != null) return;
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        Debug.Log(transform.name + ": Load TextMeshPro", gameObject);
    }

    [PunRPC]
    public void RpcReceive(float damage)
    {
        _playerDamageReceiver.Receiver(damage);
    }

    [PunRPC]
    private void SyncAnimState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Jump:
                _photonView.RPC("RpcSetTrigger", RpcTarget.Others, "jump");
                break;
            case PlayerState.Drop:
                _photonView.RPC("RpcSetTrigger", RpcTarget.Others, "drop");
                break;
            case PlayerState.Land:
                _photonView.RPC("RpcSetTrigger", RpcTarget.Others, "land");
                break;
            case PlayerState.Aim:
                _photonView.RPC("RpcSetIsAiming", RpcTarget.Others, true);
                break;
            case PlayerState.Shoot:
                _photonView.RPC("RpcShoot", RpcTarget.Others);
                break;
            case PlayerState.Dash:
                _photonView.RPC("RpcSetTrigger", RpcTarget.Others, "dash");
                break;
            case PlayerState.Die:
                _photonView.RPC("RpcSetTrigger", RpcTarget.Others, "die");
                break;
        }
    }

    [PunRPC]
    private void RpcSetTrigger(string triggerName)
    {
        _playerAnimation.Animator.SetTrigger(triggerName);
    }

    [PunRPC]
    private void RpcSetFacing(bool facingRight)
    {
        _playerAnimation.ApplyFacingRpc(facingRight);
    }

    [PunRPC]
    private void RpcSetIsAiming(bool isAiming)
    {
        _playerAnimation.SetIsAiming(isAiming);
    }

    [PunRPC]
    private void RpcShoot()
    {
        _playerAnimation.PlayShoot();
    }

    protected override void Start()
    {
        this.LoadOwnerNickName();
        if (!_photonView.IsMine) return;
        GameEvents.OnAnimStateChanged += SyncAnimState;
        GameEvents.OnPlayerFacingChanged += SyncFacing;
    }

    private void OnDestroy()
    {
        _allPlayers.Remove(this);
        GameEvents.OnAnimStateChanged -= SyncAnimState;
        GameEvents.OnPlayerFacingChanged -= SyncFacing;
    }

    private void SyncFacing(bool facingRight)
    {
        _photonView.RPC("RpcSetFacing", RpcTarget.Others, facingRight);
    }

    private void LoadOwnerNickName()
    {
        if (_photonView.ViewID == 0) return;
        this.photonNickName = _photonView.Owner.NickName;
        this._textMeshPro.text = photonNickName;
    }

    [PunRPC]
    public void RpcRevive()
    {
        GameEvents.OnPlayerRevived?.Invoke(_photonView.ViewID);
    }
}
