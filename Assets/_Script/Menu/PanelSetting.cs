using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : SaiMonoBehaviour
{
    [SerializeField] private Button _btnHome;
    [SerializeField] private Button _btnMusic;
    [SerializeField] private Button _btnExit;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnHome();
        LoadBtnMusic();
        LoadBtnExit();
    }

    protected override void Start()
    {
        base.Start();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnMusic.onClick.AddListener(OnClickMusic);
        _btnExit.onClick.AddListener(OnClickExit);
    }

    private void OnClickHome()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PhotonPlaying.instance.Leave();
    }

    private void OnClickMusic()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PanelMusic.Instance.Show();
        transform.parent.gameObject.SetActive(false);
    }

    private void OnClickExit()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        Application.Quit();
    }

    private void LoadBtnHome()
    {
        if (_btnHome != null) return;
        _btnHome = transform.Find("Home").GetComponent<Button>();
    }

    private void LoadBtnMusic()
    {
        if (_btnMusic != null) return;
        _btnMusic = transform.Find("Music").GetComponent<Button>();
    }

    private void LoadBtnExit()
    {
        if (_btnExit != null) return;
        _btnExit = transform.Find("Exit").GetComponent<Button>();
    }
}
