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
        _btnHome.onClick.AddListener(() => PhotonPlaying.instance.Leave());
        _btnMusic.onClick.AddListener(OnClickMusic);
        _btnExit.onClick.AddListener(Application.Quit);
    }

    private void OnClickMusic() { }

    private void LoadBtnHome()
    {
        if (_btnHome != null) return;
        _btnHome = transform.Find("Home").GetComponent<Button>();
        Debug.Log(transform.name + ": LoadBtnHome", gameObject);
    }

    private void LoadBtnMusic()
    {
        if (_btnMusic != null) return;
        _btnMusic = transform.Find("Music").GetComponent<Button>();
        Debug.Log(transform.name + ": LoadBtnMusic", gameObject);
    }

    private void LoadBtnExit()
    {
        if (_btnExit != null) return;
        _btnExit = transform.Find("Exit").GetComponent<Button>();
        Debug.Log(transform.name + ": LoadBtnExit", gameObject);
    }
}
