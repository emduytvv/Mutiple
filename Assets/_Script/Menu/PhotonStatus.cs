using Photon.Pun;
using TMPro;
using UnityEngine;

public class PhotonStatus : MonoBehaviourPunCallbacks
{
    public string photonStatus;
    public TextMeshProUGUI texStatus;
    void Update()
    {
        this.photonStatus = PhotonNetwork.NetworkClientState.ToString();
        this.texStatus.text = this.photonStatus;
    }
}
