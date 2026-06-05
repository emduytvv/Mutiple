using Photon.Pun;
using UnityEngine;

public class DevilCombat : BossCombat<DevilCtrl>
{
    [SerializeField] private float _skillCooldown = 10f;
    [SerializeField] private DevilPhase1Strategy _phase1;
    [SerializeField] private DevilPhase2Strategy _phase2;

    private IBossPhaseStrategy _current;
    private float _timer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPhase1();
        LoadPhase2();
    }

    private void LoadPhase1()
    {
        if (_phase1 != null) return;
        _phase1 = transform.parent.GetComponentInChildren<DevilPhase1Strategy>();
    }

    private void LoadPhase2()
    {
        if (_phase2 != null) return;
        _phase2 = transform.parent.GetComponentInChildren<DevilPhase2Strategy>();
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        _current = _phase1;
        _timer = 0f;
    }

    public void SetPhase2() => _current = _phase2;

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_bossCtrl.DamageReceiver.IsDead()) return;

        _timer += Time.deltaTime;
        if (_timer < _skillCooldown) return;
        _timer = 0f;

        SetAnimation();
    }

    private void SetAnimation()
    {
        if (_bossCtrl.DamageReceiver.IsDead()) return;
        int index = Random.Range(0, 2);
        if (index == 0) _bossCtrl.DevilAnimation.SetSkill1Trigger();
        else _bossCtrl.DevilAnimation.SetSkill2Trigger();
    }

    public void ExecuteRandomSkill() => _current?.Execute();
}
