using Photon.Pun;
using UnityEngine;

public enum GameState { Playing, GameOver, GameWin }

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private bool _test = false;
    [SerializeField] private GameState _currentState = GameState.Playing;
    public GameState CurrentState => _currentState;

    [SerializeField] private string[] _sceneOrder = { "Level1_Map1", "Level1_Map2", "Level1_Map3", "Level1_Map4", "Level1_Map5", "Level1_Map6", "Level1_Map7", "Level1_Map8" };
    [SerializeField] private float[] _statMultipliersEnemys = { 1f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f };
    [SerializeField] private int _currentMapIndex = 0;

    public float StatMultiplier => _currentMapIndex < _statMultipliersEnemys.Length
        ? _statMultipliersEnemys[_currentMapIndex]
        : 1f;

    public void SetGameState(GameState state)
    {
        _currentState = state;
        Time.timeScale = state == GameState.Playing ? 1f : 0f;
    }

    public void SetMapIndex(int index) => _currentMapIndex = index;

    protected override void Start()
    {
        base.Start();
        GameEvents.OnPlayerDied += CheckGameOver;
        GameEvents.OnBossDied += OnGameWin;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerDied -= CheckGameOver;
        GameEvents.OnBossDied -= OnGameWin;
    }

    private void CheckGameOver(int viewId)
    {
        if (_currentState != GameState.Playing) return;
        if (PlayerCtrl.AllPlayers.Count == 0) return;
        if (PlayerCtrl.AllPlayers.TrueForAll(p => p.PlayerDamageReceiver.isDead))
            OnGameOver();
    }

    protected void Update()
    {
        if (!_test) return;
        _test = false;
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        int next = _currentMapIndex + 1;
        if (next >= _sceneOrder.Length) return;
        _currentMapIndex = next;
        PhotonNetwork.LoadLevel(_sceneOrder[next]);
    }

    public void OnGameOver()
    {
        SetGameState(GameState.GameOver);
        CenterCtrl.Instance.PanelGameOver.gameObject.SetActive(true);
    }

    public void OnGameWin()
    {
        SetGameState(GameState.GameWin);
        CenterCtrl.Instance.PanelGameWin.gameObject.SetActive(true);
    }
}
