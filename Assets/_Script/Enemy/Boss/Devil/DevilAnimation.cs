using Photon.Pun;
using UnityEngine;

public class DevilAnimation : BossAnimation
{
    protected int HaseSkill_1 = Animator.StringToHash("skill_1");
    protected int HaseSkill_2 = Animator.StringToHash("skill_2");
    private DevilCtrl DevilCtrl => _bossCtrl as DevilCtrl;
    public void SetSkill1Trigger() => _animator.SetTrigger(HaseSkill_1);
    public void SetSkill2Trigger() => _animator.SetTrigger(HaseSkill_2);
    public void ExecuteByEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;  // ← THÊM
        DevilCtrl.DevilCombat.ExecuteRandomSkill();
    }
}
