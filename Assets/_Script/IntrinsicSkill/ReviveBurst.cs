using System;
using System.Collections;
using UnityEngine;

public class ReviveBurst : BaseIntrinsicSkill
{
    private Coroutine _timer;
    [SerializeField] private float _burstDuration = 15f;
    [SerializeField] private float _attackMultiplier = 0.5f;
    protected void OnEnable()
    {
        GameEvents.OnPlayerRevived += OnRevived;
    }
    protected void OnDisable()
    {
        GameEvents.OnPlayerRevived -= OnRevived;
    }
    private void OnRevived(int playerID)
    {
        if (_isActive == false) return;
        if (playerID != _player.PhotonView.ViewID) return;
        if (_timer != null)
        {
            StopCoroutine(_timer);
            AddDamage(-_attackMultiplier);
        }

        AddDamage(_attackMultiplier);
        _timer = StartCoroutine(BurstTimer());
    }

    private IEnumerator BurstTimer()
    {
        yield return new WaitForSeconds(_burstDuration);
        AddDamage(-1 * _attackMultiplier);
        _timer = null;
    }
    private void AddDamage(float attackMultiplier)
    {
        _player.PlayerDamageSender.AddPercentDamage(attackMultiplier);
    }





}

