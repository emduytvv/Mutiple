using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomAuto : MonoBehaviourPunCallbacks
{
    public string creatorNickName = "Mai";
    public string joinerNickName = "Dat";
    public string roomName = "Room1";
    [SerializeField] PhotonRoom room;

    public virtual void AutoCreateRoom()
    {
        if (PhotonNetwork.IsConnected) return;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LocalPlayer.NickName = creatorNickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(transform.name + ": OnConnectedToMaster");
        if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.CreateRoom(roomName);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message + " → JoinRoom as " + joinerNickName);
        PhotonNetwork.LocalPlayer.NickName = joinerNickName;
        PhotonNetwork.JoinRoom(roomName);
    }
}
