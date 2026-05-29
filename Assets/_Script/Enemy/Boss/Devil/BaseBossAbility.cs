using Photon.Pun;
using UnityEngine;

public abstract class BaseBossAbility : SaiMonoBehaviour
{
    [SerializeField] protected DevilCtrl _devilCtrl;

    protected bool IsPhase2 => (_devilCtrl.DamageReceiver as DevilDamageReceiver).IsPhase2;

    protected PlayerCtrl _player1;
    protected PlayerCtrl _player2;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDevilCtrl();
    }

    private void LoadDevilCtrl()
    {
        if (_devilCtrl != null) return;
        _devilCtrl = GetComponentInParent<DevilCtrl>();
    }

    protected bool SetTargets()
    {
        var players = PlayerCtrl.AllPlayers;
        if (players.Count == 0) return false;
        _player1 = players[0];
        _player2 = players.Count > 1 ? players[1] : players[0];
        return true;
    }

    public abstract void Execute();
}
