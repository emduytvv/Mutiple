using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class EnemyCreator : SaiMonoBehaviour
{
    [SerializeField] protected EnemyType _enemyType;
    public EnemyType EnemyType => _enemyType;

    [SerializeField] protected List<EnemyName> _enemyNames;

    protected virtual string GetName()
    {
        if (_enemyNames.Count == 0) return string.Empty;
        return _enemyNames[Random.Range(0, _enemyNames.Count)].ToString();
    }

    public virtual EnemyCtrl Create(Vector3 pos, Quaternion rot)
    {
        if (!PhotonNetwork.IsMasterClient) return null;
        string name = GetName();

        GameObject go = PhotonNetwork.Instantiate(name, pos, rot);
        EnemyCtrl enemy = go.GetComponent<EnemyCtrl>();
        OnCreated(enemy);
        return enemy;
    }

    protected virtual void OnCreated(EnemyCtrl enemy) { }
}
