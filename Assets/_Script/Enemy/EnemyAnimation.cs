using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : SaiMonoBehaviour
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;
    [SerializeField] protected Animator _animator;

    protected int HashDie = Animator.StringToHash("isDead");
    protected int HashHurt = Animator.StringToHash("isHurt");
    protected int HashAttack = Animator.StringToHash("attack");
    [SerializeField] protected bool _dieTriggered;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
        this.LoadAnimator();
    }

    private void LoadEnemyCtrl()
    {
        if (_enemyCtrl != null) return;
        _enemyCtrl = GetComponentInParent<EnemyCtrl>();
    }

    private void LoadAnimator()
    {
        if (_animator != null) return;
        _animator = GetComponent<Animator>();
    }

    protected void OnEnable()
    {
        _dieTriggered = false;
    }
    public void SetAttackTrigger()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        _enemyCtrl.PhotonView.RPC(nameof(EnemyCtrl.RpcSetAttackTrigger), Photon.Pun.RpcTarget.All);
    }

    public void PlayAttackAnim() => _animator.SetTrigger(HashAttack);

    public void SetHurtTrigger()
    {
        if (_enemyCtrl.DamageReceiver.isDead) return;
        _animator.SetTrigger(HashHurt);
    }
    public virtual void AttackByEvent()
    {
        _enemyCtrl.EnemyCombat.Send();
    }
    public void SetDieTrigger()
    {
        if (_dieTriggered) return;
        _dieTriggered = true;
        _animator.ResetTrigger(HashHurt);
        _animator.SetTrigger(HashDie);
    }
    public void DespawnByEvent()
    {
        _enemyCtrl.EnemyDespawn.DespawnObject();
    }
}
