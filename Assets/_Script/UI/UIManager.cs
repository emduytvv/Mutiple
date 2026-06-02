using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private CenterCtrl _center;
    [SerializeField] private Canvas _canvas;
    public CenterCtrl Center => _center;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _canvas.worldCamera = Camera.main;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCenter();
        LoadCanvas();
    }

    private void LoadCenter()
    {
        if (_center != null) return;
        _center = GetComponentInChildren<CenterCtrl>();
        Debug.Log(transform.name + ": Load CenterCtrl", gameObject);
    }

    private void LoadCanvas()
    {
        if (_canvas != null) return;
        _canvas = GetComponent<Canvas>();
        Debug.Log(transform.name + ": Load Canvas", gameObject);
    }
}
