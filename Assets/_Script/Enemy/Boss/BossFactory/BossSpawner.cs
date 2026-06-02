using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawner : Spawner
{
    private static BossSpawner _instance;
    public static BossSpawner Instance => _instance;

    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }

    protected override void ResetValue()
    {
        base.ResetValue();
        maxObject = 5;
    }
    protected void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Level1_Map8") return;
        SpawnDevil();
    }

    private void SpawnDevil()
    {
        Transform devil = SpawnByName("Devil", new Vector3(3.21f, 8.27f, 0f), Quaternion.identity);
        devil.gameObject.SetActive(true);
        DevilDamageReceiver damageReceiver = devil.GetComponentInChildren<DevilDamageReceiver>();
        DevilHPBar.Instance.SetOwnerBoss(damageReceiver);
    }

}
