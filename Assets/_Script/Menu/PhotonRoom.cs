using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] protected GameObject _mainMenu;
    public static PhotonRoom instance;
    public List<RoomProfile> rooms = new List<RoomProfile>();
    public UIRoomProfile roomPrefab;
    public List<RoomInfo> updatedRooms;
    public Transform _roomHolder;
    // void Start()
    // {
    //     nameRoom.text = "Room1";
    // }
    private void Awake()
    {
        PhotonRoom.instance = this;//Dont do this in your game
    }
    public void Create(string nameRoom)
    {
        Debug.Log("Create Room: " + nameRoom, gameObject);
        var opts = new RoomOptions { MaxPlayers = 2, IsVisible = true, IsOpen = true };
        PhotonNetwork.CreateRoom(nameRoom, opts);
    }
    public void Join(string nameRoom)
    {
        Debug.Log("Join Room: " + nameRoom, gameObject);
        PhotonNetwork.JoinRoom(nameRoom);
        // ClearRoomProfileUI();
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
    private bool _isCreator;


    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        _isCreator = true;
        CenterMenuCtrl.Instance.PanelCreateRoom.SetActive(false);
        CenterMenuCtrl.Instance.UILobby.ShowAsCreator();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        if (PhotonNetwork.IsMasterClient) return;
        _isCreator = false;
        CenterMenuCtrl.Instance.PanelJoinRoom.SetActive(false);
        CenterMenuCtrl.Instance.UILobby.ShowAsJoiner();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        UILobby.Instance.gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (UILobby.Instance == null || !UILobby.Instance.gameObject.activeSelf) return;
        if (_isCreator)
        {
            // Ta là Char1, người kia (Char2) out → ẩn slot 2
            UILobby.Instance.HideCharacter(2);
        }
        else
        {
            // Ta là Char2, creator (Char1) out → ta lên slot 1, ẩn slot 2
            var props = PhotonNetwork.CurrentRoom.CustomProperties;
            string ourChar = props.ContainsKey(UILobby.KEY_CHAR2) ? (string)props[UILobby.KEY_CHAR2] : "";
            UILobby.Instance.PromoteToSlot1(ourChar);
            _isCreator = true;
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UILobby.Instance.UpdateMasterButtons();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Char1"))
            UILobby.Instance.SyncCharacter(1, (string)changedProps["Char1"]);
        if (changedProps.ContainsKey("Char2"))
            UILobby.Instance.SyncCharacter(2, (string)changedProps["Char2"]);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
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

        roomProfile = this.GetRoomByName(roomInfo.Name);
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
            uiRoomProfile.transform.SetParent(this._roomHolder, false);
        }
    }

    protected virtual void ClearRoomProfileUI()
    {
        foreach (Transform child in this._roomHolder)
        {
            Destroy(child.gameObject);
        }
    }

    protected virtual void RoomRemove(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = this.GetRoomByName(roomInfo.Name);
        if (roomProfile == null) return;
        this.rooms.Remove(roomProfile);
    }

    protected virtual RoomProfile GetRoomByName(string name)
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

