using Photon.Pun;
using UnityEngine;

public class PhotonManager : SaiMonoBehaviour
{
    protected override void Awake()
    {
        PhotonNetwork.KeepAliveInBackground = 120;
    }
}
