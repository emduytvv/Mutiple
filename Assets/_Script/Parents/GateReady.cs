using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GateReady : SaiMonoBehaviour
{
    [SerializeField] private Transform _gate;
    [SerializeField] private PhotonView _photonView;
    private HashSet<int> _playersInside = new HashSet<int>();
    [SerializeField] private bool _isGateReady = false;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGate();
        LoadPhotonView();
    }

    private void LoadGate()
    {
        if (_gate != null) return;
        _gate = transform.Find("Gate");
    }

    private void LoadPhotonView()
    {
        if (_photonView != null) return;
        _photonView = GetComponent<PhotonView>();
    }
    protected void OnEnable()
    {
        Debug.Log(transform.name + ": OnEnable", gameObject);
        _gate.gameObject.SetActive(false);
        GameEvents.OnAllWavesCleared += OnAllWavesCleared;
    }

    protected void OnDisable()
    {
        GameEvents.OnAllWavesCleared -= OnAllWavesCleared;
    }

    private void OnAllWavesCleared()
    {
        _photonView.RPC(nameof(RpcAllWavesCleared), RpcTarget.All);
    }

    [PunRPC]
    private void RpcAllWavesCleared()
    {
        Debug.Log(transform.name + ": OnAllWavesCleared", gameObject);
        _gate.gameObject.SetActive(true);
        _isGateReady = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isGateReady) return;
        PlayerCtrl player = collision.GetComponentInParent<PlayerCtrl>();
        if (player == null || !player.PhotonView.IsMine) return;
        _photonView.RPC(nameof(RpcEnterGate), RpcTarget.All, player.PhotonView.ViewID);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isGateReady) return;
        PlayerCtrl player = collision.GetComponentInParent<PlayerCtrl>();
        if (player == null || !player.PhotonView.IsMine) return;
        _photonView.RPC(nameof(RpcExitGate), RpcTarget.All, player.PhotonView.ViewID);
    }

    [PunRPC]
    private void RpcEnterGate(int viewID)
    {
        _playersInside.Add(viewID);
        GameEvents.OnGateReadyCountChanged?.Invoke(_playersInside.Count, PlayerCtrl.AllPlayers.Count);
        if (_playersInside.Count < PlayerCtrl.AllPlayers.Count) return;
        NextScene();
    }

    [PunRPC]
    private void RpcExitGate(int viewID)
    {
        _playersInside.Remove(viewID);
        GameEvents.OnGateReadyCountChanged?.Invoke(_playersInside.Count, PlayerCtrl.AllPlayers.Count);
    }

    private void NextScene()
    {
        GameManager.Instance.LoadNextScene();
    }
}
