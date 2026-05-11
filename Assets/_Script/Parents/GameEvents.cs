using System;

public static class GameEvents
{
    public static Action OnPlayerJumped;
    public static Action OnPlayerLanded;
    public static Action<float> OnPlayerDashed;
    public static Action OnPlayerDashEnded;
    public static Action<PlayerState> OnAnimStateChanged;
    public static Action<bool> OnPlayerFacingChanged;
    public static Action OnPlayerStartAim;
    public static Action OnPlayerShoot;
    public static Action<float> OnPlayerAimAngleChanged;
    public static Action<int> OnPlayerRevived;
    public static Action<int> OnPlayerDied;

    public static Action OnEnemyDied;
    public static Action OnAllWavesCleared;

    public static Action OnEquipmentChanged;
}
