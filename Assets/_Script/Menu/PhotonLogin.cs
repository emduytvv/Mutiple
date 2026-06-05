using System;
using Firebase.Auth;
using Photon.Pun;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _setNamePanel;
    [SerializeField] private GameObject _mainMenu;

    private void Start()
    {
        CheckNewAccount();
    }

    private void CheckNewAccount()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.LogError("PhotonLogin: No Firebase user found");
            return;
        }

        // Đọc tên từ Firebase DB — async, kết quả trả về qua callback
        FirebaseDatabaseManager.Instance.ReadPlayerName(user.UserId, playerName =>
        {
            if (!string.IsNullOrEmpty(playerName))
            {
                Debug.Log("PhotonLogin: Old account");
                ConnectWithName(playerName);
            }         // tài khoản cũ
            else
            {
                Debug.Log("PhotonLogin: New account");
                _mainMenu.SetActive(false);
                _setNamePanel.SetActive(true);       // tài khoản mới → đặt tên
            }
        });
    }


    public void ConnectWithName(string playerName)
    {
        Debug.Log("Connecting as: " + playerName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() { }
}
