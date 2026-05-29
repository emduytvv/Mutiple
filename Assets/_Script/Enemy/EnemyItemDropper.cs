using Photon.Pun;
using UnityEngine;

public class EnemyItemDropper : SaiMonoBehaviour
{
    protected EnemyCtrl _enemyCtrl;
    private EnemyStatsSO _enemyStatsSO;

    protected override void Start()
    {
        _enemyStatsSO = _enemyCtrl.EnemyStatsSO;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
    }

    public void OnEnemyDead()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_enemyStatsSO == null) return;
        if (_enemyStatsSO.itemDrops == null || _enemyStatsSO.itemDrops.Count == 0) return;
        this.HandleDrop();
    }
    private void HandleDrop()
    {
        for (int i = 0; i < _enemyStatsSO.itemDrops.Count; i++)
        {
            int amount = (int)_enemyStatsSO.itemDrops[i]._rate / 100;
            if (Random.Range(0, 100) < _enemyStatsSO.itemDrops[i]._rate % 100)
                amount++;

            if (amount <= 0) continue;

            string prefabName = _enemyStatsSO.itemDrops[i].item._nameItemDrop.ToString();
            _enemyCtrl.PhotonView.RPC("RpcSpawnItemDrop", RpcTarget.All, prefabName, amount, _enemyCtrl.transform.position);
        }
    }

    public void SpawnItemDrop(string prefabName, int amount, Vector3 pos)
    {
        for (int i = 0; i < amount; i++)
            ItemDropSpawner.Instance.SpawnByCall(prefabName, pos + Vector3.up * 0.4f, Quaternion.identity);
    }

}
