using UnityEngine;

public class AutoShield : BaseIntrinsicSkill
{
    [SerializeField] private Transform _shield;
    [SerializeField] protected bool _hasShield = false;
    public bool HasShield => _hasShield;
    [SerializeField] private float _coolDown = 5f;
    [SerializeField] private float _timer = 0f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadShield();
    }

    private void LoadShield()
    {
        if (_shield != null) return;
        _shield = transform.Find("Shield");
        Debug.Log(transform.name + ": Load Shield", gameObject);
    }

    protected void Update()
    {
        if (!_isActive) return;
        CheckShieldRecharge();
    }

    private void CheckShieldRecharge()
    {
        if (_hasShield) return;
        _timer += Time.deltaTime;
        if (_timer < _coolDown) return;
        _timer = 0f;
        SetActiveShield(true);
    }

    public void SetActiveShield(bool isActive)
    {
        if (_shield == null) return;
        _hasShield = isActive;
        _shield.gameObject.SetActive(isActive);

        if (_player.PhotonView.IsMine)
            _player.PhotonView.RPC("RpcSetShield", Photon.Pun.RpcTarget.Others, isActive);
    }

}
