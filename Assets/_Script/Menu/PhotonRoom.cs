using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public static PhotonRoom instance;
    public TMP_InputField nameRoom;
    public List<RoomProfile> rooms = new List<RoomProfile>();
    public UIRoomProfile roomPrefab;
    public List<RoomInfo> updatedRooms;
    public Transform roomContent;
    void Start()
    {
        nameRoom.text = "Room1";
    }
    private void Awake()
    {
        PhotonRoom.instance = this;//Dont do this in your game
    }
    public void Create()
    {
        Debug.Log("Create Room: " + nameRoom.text, gameObject);
        PhotonNetwork.CreateRoom(nameRoom.text);
    }
    public void Join()
    {
        Debug.Log("Join Room: " + nameRoom.text, gameObject);
        PhotonNetwork.JoinRoom(nameRoom.text);
        ClearRoomProfileUI();
    }
    public virtual void Leave()
    {
        Debug.Log(transform.name + ": Leave Room");
        PhotonNetwork.LeaveRoom();
    }
    public virtual void StartGame()
    {
        Debug.Log(transform.name + ": Start Game");
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel("Level1_Map1");
        else Debug.Log("Not Master Client");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + nameRoom.text, gameObject);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room: " + nameRoom.text, gameObject);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        this.updatedRooms = roomList;

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RoomRemove(roomInfo);
            else this.RoomAdd(roomInfo);
        }

        this.UpdateRoomProfileUI();
    }

    protected virtual void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile;

        roomProfile = this.RoomByName(roomInfo.Name);
        if (roomProfile != null) return;

        roomProfile = new RoomProfile
        {
            name = roomInfo.Name
        };
        this.rooms.Add(roomProfile);

    }

    protected virtual void UpdateRoomProfileUI()
    {
        this.ClearRoomProfileUI();

        foreach (RoomProfile roomProfile in this.rooms)
        {
            UIRoomProfile uiRoomProfile = Instantiate(this.roomPrefab);
            uiRoomProfile.SetRoomProfile(roomProfile);
            uiRoomProfile.transform.SetParent(this.roomContent, false);
        }
    }

    protected virtual void ClearRoomProfileUI()
    {
        foreach (Transform child in this.roomContent)
        {
            Destroy(child.gameObject);
        }
    }

    protected virtual void RoomRemove(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = this.RoomByName(roomInfo.Name);
        if (roomProfile == null) return;
        this.rooms.Remove(roomProfile);
    }

    protected virtual RoomProfile RoomByName(string name)
    {
        foreach (RoomProfile roomProfile in this.rooms)
        {
            if (roomProfile.name == name) return roomProfile;
        }
        return null;
    }

    public virtual void OnScrollChanged()
    {
        Debug.Log("OnScrollChanged");
    }
}

