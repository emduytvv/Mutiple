using System;
using Photon.Pun;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    public Button BtnCreateRoom => _btnCreateRoom;
    [SerializeField] protected Button _btnCreateRoom;
    public Button BtnJoinRoom => _btnJoinRoom;
    [SerializeField] protected Button _btnJoinRoom;
    public Button BtnMusic => _btnMusic;
    [SerializeField] protected Button _btnMusic;
    public Button BtnExit => _btnExit;
    [SerializeField] protected Button _btnExit;
    public Button BtnLogout => _btnLogout;
    [SerializeField] protected Button _btnLogout;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnCreateRoom();
        LoadBtnJoinRoom();
        LoadBtnMusic();
        LoadBtnExit();
        LoadBtnLogout();
    }


    private void OnEnable() => AudioManager.Instance.PlayMenuMusic();

    protected override void Start()
    {
        base.Start();
        _btnCreateRoom.onClick.AddListener(OnClickCreateRoom);
        _btnJoinRoom.onClick.AddListener(OnClickJoinRoom);
        _btnMusic.onClick.AddListener(OnClickMusic);
        _btnExit.onClick.AddListener(OnClickExit);
        _btnLogout.onClick.AddListener(OnClickLogout);
    }
    private void OnClickCreateRoom()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        CenterMenuCtrl.Instance.PanelCreateRoom.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnClickJoinRoom()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        CenterMenuCtrl.Instance.PanelJoinRoom.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickMusic()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PanelMusic.Instance.Show();
        gameObject.SetActive(false);
    }

    private void OnClickExit()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        Application.Quit();
    }
    private void OnClickLogout()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        Debug.Log(transform.name + ": Logout ");
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Login");
    }
    private void LoadBtnCreateRoom()
    {
        if (_btnCreateRoom != null) return;
        _btnCreateRoom = transform.Find("Menu").Find("CreateRoom").GetComponent<Button>();
    }

    private void LoadBtnJoinRoom()
    {
        if (_btnJoinRoom != null) return;
        _btnJoinRoom = transform.Find("Menu").Find("JoinRoom").GetComponent<Button>();
    }

    private void LoadBtnMusic()
    {
        if (_btnMusic != null) return;
        _btnMusic = transform.Find("Menu").Find("Music").GetComponent<Button>();
    }
    private void LoadBtnLogout()
    {
        if (_btnLogout != null) return;
        _btnLogout = transform.Find("Menu").Find("Logout").GetComponent<Button>();
    }

    private void LoadBtnExit()
    {
        if (_btnExit != null) return;
        _btnExit = transform.Find("Menu").Find("Exit").GetComponent<Button>();
    }
}
