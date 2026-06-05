using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : SaiMonoBehaviour
{
    [SerializeField] private Button _btnBuffGold;
    [SerializeField] private Button _btnBuffHP;
    [SerializeField] private Button _btnNextMap;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnBuffGold();
        LoadBtnBuffHP();
        LoadBtnNextMap();
    }

    private void LoadBtnBuffGold()
    {
        if (_btnBuffGold != null) return;
        _btnBuffGold = transform.Find("BuffGold")?.GetComponent<Button>();
    }

    private void LoadBtnBuffHP()
    {
        if (_btnBuffHP != null) return;
        _btnBuffHP = transform.Find("BuffHP")?.GetComponent<Button>();
    }

    private void LoadBtnNextMap()
    {
        if (_btnNextMap != null) return;
        _btnNextMap = transform.Find("NextMap")?.GetComponent<Button>();
    }

    protected override void Start()
    {
        base.Start();
        _btnBuffGold.onClick.AddListener(OnBuffGold);
        _btnBuffHP.onClick.AddListener(OnBuffHP);
        _btnNextMap.onClick.AddListener(OnNextMap);
    }

    private PlayerCtrl GetMyPlayer() => PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);

    private void OnBuffGold()
    {
        PlayerCtrl player = GetMyPlayer();
        if (player == null) return;
        player.GetComponentInChildren<PlayerGold>().AddGold(1000);
    }

    private void OnBuffHP()
    {
        PlayerCtrl player = GetMyPlayer();
        if (player == null) return;
        player.PhotonView.RPC("RpcBuff", RpcTarget.All, 100f);
    }

    private void OnNextMap()
    {
        GameManager.Instance.LoadNextScene();
    }
}
