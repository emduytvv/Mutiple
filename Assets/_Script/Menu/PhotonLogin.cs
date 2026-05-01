using Photon.Pun;
using TMPro;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputUsername;
    void Start()
    {
        inputUsername.text = "Duy";
    }
    public virtual void Login()
    {
        string name = inputUsername.text;
        Debug.Log("Login: " + name);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = name;
        PhotonNetwork.ConnectUsingSettings();
    }
    //Được gọi khi ConnectUsingSettings
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }
    //Được gọi khi JoinLobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
}
