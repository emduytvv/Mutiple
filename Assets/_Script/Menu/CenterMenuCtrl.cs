using System;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterMenuCtrl : Singleton<CenterMenuCtrl>
{
    [SerializeField] protected GameObject _panelCreateRoom;
    public GameObject PanelCreateRoom => _panelCreateRoom;
    [SerializeField] protected GameObject _panelJoinRoom;
    public GameObject PanelJoinRoom => _panelJoinRoom;
    [SerializeField] protected UILobby _uILobby;
    public UILobby UILobby => _uILobby;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPanelCreateRoom();
        LoadPanelJoinRoom();
        LoadUILobby();
    }

    private void LoadPanelCreateRoom()
    {
        if (_panelCreateRoom != null) return;
        _panelCreateRoom = transform.Find("PanelCreateRoom").gameObject;
        _panelCreateRoom.SetActive(false);
        Debug.Log(transform.name + ": Load PanelCreateRoom", gameObject);
    }

    private void LoadPanelJoinRoom()
    {
        if (_panelJoinRoom != null) return;
        _panelJoinRoom = transform.Find("PanelJoinRoom").gameObject;
        _panelJoinRoom.SetActive(false);
        Debug.Log(transform.name + ": Load PanelJoinRoom", gameObject);
    }
    private void LoadUILobby()
    {
        if (_uILobby != null) return;
        _uILobby = transform.Find("UILobby").GetComponent<UILobby>();
        _uILobby.gameObject.SetActive(false);
        Debug.Log(transform.name + ": Load LoadUILobby", gameObject);
    }
}
