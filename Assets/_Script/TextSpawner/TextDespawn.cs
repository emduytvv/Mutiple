using Photon.Pun;
using UnityEngine;

public class TextDespawn : DespawnByTime
{
    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 0.5f;
    }

    public override void DespawnObject()
    {
        TextSpawner.Instance.Despawn(transform.parent);
    }
}
