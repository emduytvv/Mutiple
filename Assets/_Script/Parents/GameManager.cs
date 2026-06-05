using Photon.Pun;
using UnityEngine;

public enum GameState { Playing, GameOver, GameWin }

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private bool _test = false;
    [SerializeField] private GameState _currentState = GameState.Playing;
    public GameState CurrentState => _currentState;

    [SerializeField] private string[] _sceneOrder = { "Level1_Map1", "Level1_Map2", "Level1_Map3", "Level1_Map4", "Level1_Map5", "Level1_Map6", "Level1_Map7", "Level1_Map8" };
    [SerializeField] private float[] _statMultipliersEnemys = { 1f, 1.1f, 1.22f, 1.36f, 1.52f, 1.70f, 1.90f, 2.10f };
    [SerializeField] private int _currentMapIndex = 0;

    public float StatMultiplier => _currentMapIndex < _statMultipliersEnemys.Length
        ? _statMultipliersEnemys[_currentMapIndex]
        : 1f;

    public void SetGameState(GameState state)
    {
        _currentState = state;
    }

    public void SetMapIndex(int index) => _currentMapIndex = index;

    protected override void Start()
    {
        base.Start();
        GameEvents.OnPlayerDied += CheckGameOver;
        GameEvents.OnBossDied += OnGameWin;
        AudioManager.Instance.PlayBattleMusic();
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
