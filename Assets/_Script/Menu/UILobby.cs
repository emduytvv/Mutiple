using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : Singleton<UILobby>
{
    // Key lÆ°u trong Room Custom Properties Ä‘á»ƒ sync nhÃ¢n váº­t giá»¯a 2 mÃ¡y
    public const string KEY_CHAR1 = "Char1";
    public const string KEY_CHAR2 = "Char2";

    public Button LeaveRoomBtn => _leaveRoomBtn;
    [SerializeField] protected Button _leaveRoomBtn;

    public Button StartBtn => _startBtn;
    [SerializeField] protected Button _startBtn;
    public Button SelectLevelBtn => _selectLevelBtn;
    [SerializeField] protected Button _selectLevelBtn;

    public GameObject Character1 => _character1;
    [SerializeField] protected GameObject _character1;

    public GameObject Character2 => _character2;
    [SerializeField] protected GameObject _character2;

    [SerializeField] private UICharacterMenu _uiCharacterMenu;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLeaveRoomBtn();
        LoadStartBtn();
        LoadSelectLevelBtn();
        LoadCharacter1();
        LoadCharacter2();
        LoadUICharacterMenu();
    }

    private void OnEnable() => AudioManager.Instance.PlayLobbyMusic();

    protected override void Start()
    {
        base.Start();
        _startBtn.onClick.AddListener(OnClickStart);
        _leaveRoomBtn.onClick.AddListener(OnClickLeaveRoom);
    }

    private void OnClickStart()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PhotonRoom.instance.StartGame();
    }

    private void OnClickLeaveRoom()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PhotonRoom.instance.Leave();
    }

    private void LoadLeaveRoomBtn()
    {
        if (_leaveRoomBtn != null) return;
        _leaveRoomBtn = transform.Find("LeaveRoom").GetComponent<Button>();
    }

    private void LoadStartBtn()
    {
        if (_startBtn != null) return;
        _startBtn = transform.Find("Start").GetComponent<Button>();
    }

    private void LoadSelectLevelBtn()
    {
        if (_selectLevelBtn != null) return;
        _selectLevelBtn = transform.Find("SelectLevel").GetComponent<Button>();
    }

    private void LoadCharacter1()
    {
        if (_character1 != null) return;
        _character1 = transform.Find("Character_1").gameObject;
    }

    private void LoadCharacter2()
    {
        if (_character2 != null) return;
        _character2 = transform.Find("Character_2").gameObject;
    }

    private void LoadUICharacterMenu()
    {
        if (_uiCharacterMenu != null) return;
        _uiCharacterMenu = transform.parent.GetComponentInChildren<UICharacterMenu>();
    }

    public void ShowAsCreator()
    {
        gameObject.SetActive(true);
        _character1.SetActive(true);
        _character2.SetActive(false);
        string charName = _uiCharacterMenu.GetCharacterNameSelected();
        SetCharacterDisplay(_character1, charName);
        BroadcastCharacter(KEY_CHAR1, charName);
        UpdateMasterButtons();
    }
    public void ShowAsJoiner()
    {
        gameObject.SetActive(true);
        _character2.SetActive(true);

        var props = PhotonNetwork.CurrentRoom.CustomProperties;
        _character1.SetActive(props.ContainsKey(KEY_CHAR1));
        if (props.ContainsKey(KEY_CHAR1))
            SetCharacterDisplay(_character1, (string)props[KEY_CHAR1]);

        string charName = _uiCharacterMenu.GetCharacterNameSelected();
        SetCharacterDisplay(_character2, charName);
        BroadcastCharacter(KEY_CHAR2, charName);
        UpdateMasterButtons();
    }
    public void SyncCharacter(int slot, string charName)
    {
        GameObject panel = slot == 1 ? _character1 : _character2;
        panel.SetActive(true);
        SetCharacterDisplay(panel, charName);
    }

    public void HideCharacter(int slot)
    {
        GameObject panel = slot == 1 ? _character1 : _character2;
        panel.SetActive(false);
    }

    public void PromoteToSlot1(string charName)
    {
        SetCharacterDisplay(_character1, charName);
        _character1.SetActive(true);
        _character2.SetActive(false);
        BroadcastCharacter(KEY_CHAR1, charName);
    }

    public void UpdateMasterButtons()
    {
        bool isMaster = PhotonNetwork.IsMasterClient;
        _startBtn.interactable = isMaster;
        _selectLevelBtn.interactable = isMaster;
    }

    private void BroadcastCharacter(string key, string charName)
    {
        if (!PhotonNetwork.InRoom) return;
        Hashtable props = new Hashtable { { key, charName } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    private void SetCharacterDisplay(GameObject panel, string characterName)
    {
        foreach (Transform child in panel.transform)
        {
            string n = child.name;
            if (n == "Base") continue;
            child.gameObject.SetActive(n == characterName);
        }
    }
}
