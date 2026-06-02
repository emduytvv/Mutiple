using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetName : SaiMonoBehaviour
{
    [SerializeField] private TMP_InputField _inputName;
    [SerializeField] private Button _btnConfirm;
    [SerializeField] private PhotonLogin _photonLogin;
    [SerializeField] private GameObject _mainMenu;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadInputName();
        LoadBtnConfirm();
        LoadPhotonLogin();
    }
    protected override void Awake()
    {
        base.Awake();
        _btnConfirm.onClick.AddListener(OnConfirm);
    }

    private void LoadInputName()
    {
        if (_inputName != null) return;
        _inputName = GetComponentInChildren<TMP_InputField>();
        Debug.Log(transform.name + ": Load InputName", gameObject);
    }

    private void LoadBtnConfirm()
    {
        if (_btnConfirm != null) return;
        _btnConfirm = transform.Find("Confirm").GetComponent<Button>();
        Debug.Log(transform.name + ": Load BtnConfirm", gameObject);
    }

    private void LoadPhotonLogin()
    {
        if (_photonLogin != null) return;
        _photonLogin = transform.parent.GetComponentInChildren<PhotonLogin>();
        Debug.Log(transform.name + ": Load PhotonLogin", gameObject);
    }

    private void OnConfirm()
    {
        string playerName = _inputName.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabaseManager.Instance.SavePlayerName(uid, playerName);
        _photonLogin.ConnectWithName(playerName);

        _mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
