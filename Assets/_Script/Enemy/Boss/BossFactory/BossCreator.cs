using Photon.Pun;
using UnityEngine;

public abstract class BossCreator : SaiMonoBehaviour
{
    [SerializeField] protected BossName _bossName;

    public virtual BossCtrl Create(Vector3 pos, Quaternion rot)
    {
        if (!PhotonNetwork.IsMasterClient) return null;
        string name = _bossName.ToString();
        GameObject go = PhotonNetwork.Instantiate(name, pos, rot);
        BossCtrl boss = go.GetComponent<BossCtrl>();
        OnCreated(boss);
        return boss;
    }

    protected virtual void OnCreated(BossCtrl boss) { }
}
