using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelJoinRoom : SaiMonoBehaviour
{
    private string _inputRoomName = "NoRoom";
    [SerializeField] private Button _buttonJoinRoom;
    [SerializeField] private Button _buttonBack;
    [SerializeField] private Transform _mainMenu;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadButton();
        this.LoadButtonBack();
    }

    private void LoadButtonBack()
    {
        if (_buttonBack != null) return;
        _buttonBack = transform.Find("Back").GetComponent<Button>();
    }
    public void SetInputRoomName(string inputRoomName) => _inputRoomName = inputRoomName;
    private void LoadButton()
    {
        if (_buttonJoinRoom != null) return;
        _buttonJoinRoom = transform.Find("Join").GetComponent<Button>();
    }
    protected override void Awake()
    {
        base.Awake();
        _buttonBack.onClick.AddListener(Back);
        _buttonJoinRoom.onClick.AddListener(Join);
    }
    private void Join()
    {
        if (_inputRoomName == "NoRoom") return;
        PhotonRoom.instance.Join(_inputRoomName);
    }
    private void Back()
    {
        _mainMenu.gameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
