using UnityEngine;

public class PlayerSpawnPoint : SaiMonoBehaviour
{
    private static PlayerSpawnPoint _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    public static Vector3 Get()
    {
        if (_instance == null)
        {
            Debug.LogWarning("PlayerSpawnPoint: chưa có trong scene");
            return Vector3.zero;
        }
        return _instance.transform.position;
    }
}
