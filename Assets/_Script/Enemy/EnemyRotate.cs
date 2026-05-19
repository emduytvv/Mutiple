using Unity.Mathematics;
using UnityEngine;

public class EnemyRotate : SaiMonoBehaviour
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    protected void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (math.abs(_enemyCtrl.Rigidbody2D.linearVelocity.x) < 0.01f) return;
        transform.parent.localScale = new Vector3(_enemyCtrl.Rigidbody2D.linearVelocity.x < 0 ? -1 : 1, 1, 1);
    }
}
