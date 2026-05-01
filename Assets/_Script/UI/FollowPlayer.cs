using UnityEngine;

// Giữ World Space Canvas luôn quay mặt về camera (billboard).
// Đặt component này trên HP bar Canvas, là child của player prefab.
// Không cần biết player nào — GetComponentInParent tự tìm đúng parent của nó.
public class FollowPlayer : SaiMonoBehaviour
{
    [SerializeField] private Camera _camera;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (_camera != null) return;
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null) return;
        transform.rotation = _camera.transform.rotation;
    }
}
