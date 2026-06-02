using Photon.Pun;
using TMPro;
using UnityEngine;

public class UIRoomProfile : BaseBtn
{
    [SerializeField] protected TextMeshProUGUI roomName;
    [SerializeField] protected RoomProfile roomProfile;

    public virtual void SetRoomProfile(RoomProfile roomProfile)
    {
        this.roomProfile = roomProfile;
        this.roomName.text = this.roomProfile.name;
    }

    protected override void OnClick()
    {
        Debug.Log("OnClick: " + this.roomProfile.name);
        transform.GetComponentInParent<PanelJoinRoom>().SetInputRoomName(this.roomProfile.name);
    }
}
