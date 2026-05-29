using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonPlaying : MonoBehaviourPunCallbacks
{
    public static PhotonPlaying instance;
    public string ModelName1 = "Raidon";
    public string ModelName2 = "Raidon";
    private CinemachineCamera _cinemachineCamera;

    public List<PlayerProfile> players = new List<PlayerProfile>();

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadPlayers();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        Invoke(nameof(SpawnPlayer), 0.1f);
    }

    protected virtual void SpawnPlayer()
    {
        PlayerCtrl existing = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
        if (existing != null)
        {
            existing.transform.position = PlayerSpawnPoint.Get();
            _cinemachineCamera.Target.TrackingTarget = existing.transform;
            return;
        }
        this.LoadPlayerPrefab();
    }

    private int GetLocalPlayerIndex()
    {
        List<int> actorNumbers = new(PhotonNetwork.CurrentRoom.Players.Keys);
        actorNumbers.Sort();
        return actorNumbers.IndexOf(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    private void LoadPlayers()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke(nameof(LoadPlayers), 1f);
            return;
        }
        foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(playerData.Value.NickName);
            PlayerProfile playerProfile = new() { nickName = playerData.Value.NickName };
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
        int playerIndex = GetLocalPlayerIndex();
        string prefabName = playerIndex == 0 ? ModelName1 : ModelName2;
        GameObject player = PhotonNetwork.Instantiate(prefabName, PlayerSpawnPoint.Get(), Quaternion.identity);
        if (player.GetComponent<PhotonView>().IsMine)
            _cinemachineCamera.Target.TrackingTarget = player.transform;
    }
}
