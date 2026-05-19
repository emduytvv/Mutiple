using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : SaiMonoBehaviour
{
    [SerializeField] protected EnemyCtrl _enemyCtrl;
    [SerializeField] protected Animator _animator;

    static readonly int HashDie = Animator.StringToHash("isDead");
    static readonly int HashHurt = Animator.StringToHash("isHurt");
    static readonly int HashAttack = Animator.StringToHash("attack");
    [SerializeField] private bool _dieTriggered;

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
        Debug.Log(transform.name + ": Load EnemyCtrl", gameObject);
    }

    private void LoadAnimator()
    {
        if (_animator != null) return;
        _animator = GetComponent<Animator>();
        Debug.Log(transform.name + ": Load Animator", gameObject);
    }

    protected void OnEnable()
    {
        _dieTriggered = false;
    }

    private void Update()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        HandleDeadAnim();
    }

    public void SetAttackTrigger()
    {
        _animator.SetTrigger(HashAttack);
    }

    public void OnHurt()
    {
        if (!_enemyCtrl.PhotonView.IsMine) return;
        if (_enemyCtrl.DamageReceiver.isDead) return;
        _animator.SetTrigger(HashHurt);
    }

    private void HandleDeadAnim()
    {
        if (_dieTriggered || !_enemyCtrl.DamageReceiver.isDead) return;
        _dieTriggered = true;
        _animator.ResetTrigger(HashHurt);
        _animator.SetTrigger(HashDie);
    }

    public void DespawnByEvent()
    {
        _enemyCtrl.EnemyDespawn.DespawnObject();
    }
}
