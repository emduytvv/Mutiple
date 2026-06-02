using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : Singleton<UILobby>
{
    // Key lưu trong Room Custom Properties để sync nhân vật giữa 2 máy
    private const string KEY_CHAR1 = "Char1";
    private const string KEY_CHAR2 = "Char2";

    public Button LeaveRoomBtn => _leaveRoomBtn;
    [SerializeField] protected Button _leaveRoomBtn;

    public Button StartBtn => _startBtn;
    [SerializeField] protected Button _startBtn;

    // Chỉ master client mới interactable, guest thấy greyed-out
    public Button SelectLevelBtn => _selectLevelBtn;
    [SerializeField] protected Button _selectLevelBtn;

    public GameObject Character1 => _character1;
    [SerializeField] protected GameObject _character1;

    public GameObject Character2 => _character2;
    [SerializeField] protected GameObject _character2;

    // UICharacterMenu ở màn hình chọn nhân vật trước khi vào phòng
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

    protected override void Start()
    {
        base.Start();
        _startBtn.onClick.AddListener(() => PhotonRoom.instance.StartGame());
        _leaveRoomBtn.onClick.AddListener(() => PhotonRoom.instance.Leave());
    }

    private void LoadLeaveRoomBtn()
    {
        if (_leaveRoomBtn != null) return;
        _leaveRoomBtn = transform.Find("LeaveRoom").GetComponent<Button>();
        Debug.Log(transform.name + ": Load LeaveRoomBtn", gameObject);
    }

    private void LoadStartBtn()
    {
        if (_startBtn != null) return;
        _startBtn = transform.Find("Start").GetComponent<Button>();
        Debug.Log(transform.name + ": Load StartBtn", gameObject);
    }

    private void LoadSelectLevelBtn()
    {
        if (_selectLevelBtn != null) return;
        _selectLevelBtn = transform.Find("SelectLevel").GetComponent<Button>();
        Debug.Log(transform.name + ": Load SelectLevelBtn", gameObject);
    }

    private void LoadCharacter1()
    {
        if (_character1 != null) return;
        _character1 = transform.Find("Character_1").gameObject;
        Debug.Log(transform.name + ": Load Character1", gameObject);
    }

    private void LoadCharacter2()
    {
        if (_character2 != null) return;
        _character2 = transform.Find("Character_2").gameObject;
        Debug.Log(transform.name + ": Load Character2", gameObject);
    }

    private void LoadUICharacterMenu()
    {
        if (_uiCharacterMenu != null) return;
        _uiCharacterMenu = transform.parent.GetComponentInChildren<UICharacterMenu>();
        Debug.Log(transform.name + ": Load UICharacterMenu", gameObject);
    }

    // Gọi khi tạo phòng: hiện Character_1, ẩn Character_2 (chờ người join)
    public void ShowAsCreator()
    {
        gameObject.SetActive(true);
        _character1.SetActive(true);
        _character2.SetActive(false);
        string charName = _uiCharacterMenu.GetCharacterNameSelected();
        SetCharacterDisplay(_character1, charName);
        BroadcastCharacter(KEY_CHAR1, charName); // gửi lên room props để joiner đọc
        UpdateMasterButtons();
    }

    // Gọi khi join phòng: hiện Character_2, đọc Character_1 từ room props có sẵn
    public void ShowAsJoiner()
    {
        gameObject.SetActive(true);
        _character2.SetActive(true);

        // Room props đã có Char1 từ lúc creator tạo phòng → đọc thẳng, không cần chờ callback
        var props = PhotonNetwork.CurrentRoom.CustomProperties;
        _character1.SetActive(props.ContainsKey(KEY_CHAR1));
        if (props.ContainsKey(KEY_CHAR1))
            SetCharacterDisplay(_character1, (string)props[KEY_CHAR1]);

        string charName = _uiCharacterMenu.GetCharacterNameSelected();
        SetCharacterDisplay(_character2, charName);
        BroadcastCharacter(KEY_CHAR2, charName); // gửi lên room props để creator nhận qua OnRoomPropertiesUpdate
        UpdateMasterButtons();
    }

    // Gọi từ PhotonRoom.OnRoomPropertiesUpdate khi phía còn lại thay đổi nhân vật
    public void SyncCharacter(int slot, string charName)
    {
        GameObject panel = slot == 1 ? _character1 : _character2;
        panel.SetActive(true);
        SetCharacterDisplay(panel, charName);
    }

    // Start/SelectLevel chỉ interactable với master, guest thấy button mờ không bấm được
    public void UpdateMasterButtons()
    {
        bool isMaster = PhotonNetwork.IsMasterClient;
        _startBtn.interactable = isMaster;
        _selectLevelBtn.interactable = isMaster;
    }

    // Lưu tên nhân vật vào Room Custom Properties để sync sang tất cả client trong phòng
    private void BroadcastCharacter(string key, string charName)
    {
        Hashtable props = new Hashtable { { key, charName } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    // Bật đúng nhân vật (Raidon/Velvet) trong panel, tắt các nhân vật còn lại
    private void SetCharacterDisplay(GameObject panel, string characterName)
    {
        foreach (Transform child in panel.transform)
        {
            string n = child.name;
            if (n == "Base") continue; // giữ nguyên background/base
            child.gameObject.SetActive(n == characterName);
        }
    }
}
