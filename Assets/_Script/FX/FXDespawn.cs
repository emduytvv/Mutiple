public class FXDespawn : DespawnByTime
{
    protected override void ResetValue()
    {
        base.ResetValue();
        timeDespawn = 1f;
    }
}
