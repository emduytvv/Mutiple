using Photon.Pun;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override bool Persistent => true;

    [SerializeField] private string[] _sceneOrder = { "Level1_Map1", "Level1_Map2" };
    [SerializeField] private float[] _statMultipliersEnemys = { 1f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f };
    [SerializeField] private int _currentMapIndex = 0;

    public float StatMultiplier => _currentMapIndex < _statMultipliersEnemys.Length
        ? _statMultipliersEnemys[_currentMapIndex]
        : 1f;

    public void SetMapIndex(int index) => _currentMapIndex = index;

    public void LoadNextScene()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        int next = _currentMapIndex + 1;
        if (next >= _sceneOrder.Length) return;
        _currentMapIndex = next;
        PhotonNetwork.LoadLevel(_sceneOrder[next]);
    }
}
