using UnityEngine;

public class DespawnByTime : Despawn
{
    [SerializeField] protected float timer = 0f;
    [SerializeField] protected float timeDespawn = 1f;

    protected virtual void OnEnable()
    {
        timer = 0f;
    }

    protected override bool CanDespawn()
    {
        timer += Time.fixedDeltaTime;
        if (timer < timeDespawn) return false;
        timer = 0f;
        return true;
    }
}
