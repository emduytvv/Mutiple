using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonPlaying : MonoBehaviourPunCallbacks
{
    public static PhotonPlaying instance;
    public string ModelName1 = "Raidon";
    public string ModelName2 = "Raidon";

    public List<PlayerProfile> players = new List<PlayerProfile>();
    private void Awake()
    {
        PhotonPlaying.instance = this;//Dont do this in your game
        LoadPlayers();
    }
    void Start()
    {
        Invoke(nameof(SpawnPlayer), 0.1f);
    }
    protected virtual void SpawnPlayer()
    {

        this.LoadPlayerPrefab();


        // GameObject playerObj = Resources.Load(this.photonPlayerName) as GameObject;
        // Instantiate(playerObj);
    }

    private void LoadPlayers()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke(nameof(LoadPlayers), 1f);
            return;
        }

        PlayerProfile playerProfile;
        foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(playerData.Value.NickName);
            playerProfile = new PlayerProfile
            {
                nickName = playerData.Value.NickName
            };
            this.players.Add(playerProfile);
        }
    }

    public virtual void Leave()
    {
        Debug.Log(transform.name + ": Leave Room");
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        PhotonNetwork.LoadLevel("SampleScene");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }
    protected virtual void LoadPlayerPrefab()
    {
        List<int> actorNumbers = new(PhotonNetwork.CurrentRoom.Players.Keys);
        actorNumbers.Sort();
        int playerIndex = actorNumbers.IndexOf(PhotonNetwork.LocalPlayer.ActorNumber);
        string prefabName = playerIndex == 0 ? ModelName1 : ModelName2;
        PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);
    }
}

