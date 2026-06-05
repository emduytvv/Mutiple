using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelCreateRoom : SaiMonoBehaviour
{
    [SerializeField] private TMP_InputField _inputRoomName;
    [SerializeField] private Button _buttonCreateRoom;
    [SerializeField] private Button _buttonBack;
    [SerializeField] private Transform _mainMenu;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadInputField();
        this.LoadButton();
        this.LoadButtonBack();
    }

    private void LoadButtonBack()
    {
        if (_buttonBack != null) return;
        _buttonBack = transform.Find("Back").GetComponent<Button>();
    }

    private void LoadButton()
    {
        if (_buttonCreateRoom != null) return;
        _buttonCreateRoom = transform.Find("Create").GetComponent<Button>();
    }

    private void LoadInputField()
    {
        if (_inputRoomName != null) return;
        _inputRoomName = GetComponentInChildren<TMP_InputField>();
    }

    protected override void Awake()
    {
        base.Awake();
        _buttonCreateRoom.onClick.AddListener(OnClickCreate);
        _buttonBack.onClick.AddListener(Back);
    }

    private void OnClickCreate()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PhotonRoom.instance.Create(_inputRoomName.text);
    }

    private void Back()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        _mainMenu.gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
}
