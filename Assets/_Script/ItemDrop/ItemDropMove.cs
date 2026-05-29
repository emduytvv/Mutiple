using UnityEngine;

public class ItemDropMove : Movement
{
    protected ItemDropCtrl _itemDropCtrl;
    [SerializeField] private Transform _target;
    protected float _forceHorrizontal = 4f;
    protected float _forceVertical = 5.2f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadItemDropCtrl();
    }
    protected override void ResetValue()
    {
        base.ResetValue();
        _moveSpeed = 8f;
    }

    private void LoadItemDropCtrl()
    {
        if (_itemDropCtrl != null) return;
        _itemDropCtrl = GetComponentInParent<ItemDropCtrl>();
    }

    private void OnEnable()
    {
        _target = null;
        _itemDropCtrl.Rigidbody2D.simulated = true;
        MoveNotTarget();
    }

    public void SetTarget(Transform target) => _target = target;

    protected override void Move()
    {
        if (_target == null) return;
        transform.parent.position = Vector2.MoveTowards(transform.parent.position, _target.position + Vector3.up * 0.5f, _moveSpeed * Time.fixedDeltaTime);

    }
    protected void MoveNotTarget()
    {
        float x = Random.Range(-_forceHorrizontal, _forceHorrizontal);
        Vector3 dir = new Vector3(x, _forceVertical, 0);
        _itemDropCtrl.Rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
    }
}
