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
    public PlayerDamageSender PlayerDamageSender => _playerDamageSender;
    [SerializeField] protected PlayerDamageSender _playerDamageSender;
    public PlayerDespawn PlayerDespawn => _playerDespawn;
    [SerializeField] protected PlayerDespawn _playerDespawn;
    public EquipmentManager EquipmentManager => _equipmentManager;
    [SerializeField] protected EquipmentManager _equipmentManager;
    public PlayerPickup PlayerPickup => _playerPickup;
    [SerializeField] protected PlayerPickup _playerPickup;
    public AutoShield AutoShield => _autoShield;
    [SerializeField] protected AutoShield _autoShield;
    public InventoryManager InventoryManager => _inventoryManager;
    [SerializeField] protected InventoryManager _inventoryManager;
    public PlayerPowerUpManager PlayerPowerUpManager => _playerPowerUpManager;
    [SerializeField] protected PlayerPowerUpManager _playerPowerUpManager;
    public PlayerItemTransfer PlayerItemTransfer => _playerItemTransfer;
    [SerializeField] protected PlayerItemTransfer _playerItemTransfer;

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
        this.LoadPlayerDamageSender();
        this.LoadPlayerDespawn();
        this.LoadTextMeshPro();
        this.LoadEquipmentManager();
        this.LoadPlayerPickup();
        this.LoadAutoShield();
        this.LoadInventoryManager();
        this.LoadPlayerPowerUpManager();
        this.LoadPlayerItemTransfer();
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
    private void LoadPlayerDamageSender()
    {
        if (_playerDamageSender != null) return;
        _playerDamageSender = GetComponentInChildren<PlayerDamageSender>();
        Debug.Log(transform.name + ": LoadPlayerDamageSender", gameObject);
    }
    private void LoadEquipmentManager()
    {
        if (_equipmentManager != null) return;
        _equipmentManager = GetComponentInChildren<EquipmentManager>();
        Debug.Log(transform.name + ": LoadEquipmentManager", gameObject);
    }

    private void LoadPlayerPickup()
    {
        if (_playerPickup != null) return;
        _playerPickup = GetComponentInChildren<PlayerPickup>();
    }

    private void LoadAutoShield()
    {
        if (_autoShield != null) return;
        _autoShield = GetComponentInChildren<AutoShield>();
    }

    private void LoadInventoryManager()
    {
        if (_inventoryManager != null) return;
        _inventoryManager = GetComponentInChildren<InventoryManager>();
        Debug.Log(transform.name + ": Load InventoryManager", gameObject);
    }

    private void LoadPlayerPowerUpManager()
    {
        if (_playerPowerUpManager != null) return;
        _playerPowerUpManager = GetComponentInChildren<PlayerPowerUpManager>();
        Debug.Log(transform.name + ": Load PlayerPowerUpManager", gameObject);
    }

    private void LoadPlayerItemTransfer()
    {
        if (_playerItemTransfer != null) return;
        _playerItemTransfer = GetComponentInChildren<PlayerItemTransfer>();
        Debug.Log(transform.name + ": Load PlayerItemTransfer", gameObject);
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
    public void RpcReceive(float physDamage, float magDamage)
    {
        _playerDamageReceiver.Receiver(physDamage, magDamage);
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
    private void RpcSetAimAngle(float angle)
    {
        _playerAnimation.SetAimAngle(angle);
    }

    [PunRPC]
    private void RpcShoot()
    {
        _playerAnimation.PlayShoot();
    }

    [PunRPC]
    private void RpcSetShield(bool isActive)
    {
        _autoShield.SetActiveShield(isActive);
    }

    [PunRPC]
    private void RpcAddMaxHP(float amount) => _playerDamageReceiver.AddMaxHP(amount);

    [PunRPC]
    private void RpcAddPercentHP(float percent) => _playerDamageReceiver.AddPercentHPBonus(percent);

    [PunRPC]
    private void RpcBuffPercentHP(float percent) => _playerDamageReceiver.BuffPercentHP(percent);

    [PunRPC]
    private void RpcAddDefense(float amount)
    {
        _playerDamageReceiver.AddPhysicalDefense(amount);
        _playerDamageReceiver.AddMagicalDefense(amount);
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
    private void RpcReceiveItem(string json)
    {
        _playerItemTransfer.ReceiveItem(json);
    }

    [PunRPC]
    public void RpcRevive()
    {
        GameEvents.OnPlayerRevived?.Invoke(_photonView.ViewID);
    }

    [PunRPC]
    private void RpcSyncDefenseStats(float physDef, float magDef, float hp)
    {
        _playerDamageReceiver.ApplyEquipmentDefenseBonus(physDef, magDef, hp);
    }
}
