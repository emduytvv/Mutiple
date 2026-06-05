using UnityEngine;
using UnityEngine.UI;

public class PanelGameOver : SaiMonoBehaviour
{
    [SerializeField] private Button _btnHome;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnHome();
    }

    protected override void Start()
    {
        base.Start();
        _btnHome.onClick.AddListener(OnClickHome);
    }

    private void OnClickHome()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        PhotonPlaying.instance.Leave();
    }

    private void LoadBtnHome()
    {
        if (_btnHome != null) return;
        _btnHome = transform.Find("Home").GetComponent<Button>();
    }
}
