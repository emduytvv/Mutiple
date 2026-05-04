using UnityEngine;

public class CameraFollow : SaiMonoBehaviour
{
    [SerializeField] protected float smoothSpeed = 5f;
    protected Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    protected virtual void LateUpdate()
    {
        if (this.target == null) return;
        Vector3 desired = this.target.position + Vector3.up * 3f;
        transform.position = Vector3.Lerp(transform.position, desired, this.smoothSpeed * Time.deltaTime);
    }
}
