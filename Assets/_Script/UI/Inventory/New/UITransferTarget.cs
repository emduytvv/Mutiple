using UnityEngine;
using UnityEngine.UI;

// Drop zone màu vàng trong inventory panel — kéo item vào đây để chuyển cho teammate
public class UITransferTarget : SaiMonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCanvasGroup();
    }

    private void LoadCanvasGroup()
    {
        if (_canvasGroup != null) return;
        _canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log(transform.name + ": Load CanvasGroup", gameObject);
    }

    private void FixedUpdate()
    {
        bool hasTeammate = PlayerCtrl.AllPlayers.Count > 1;
        _canvasGroup.alpha = hasTeammate ? 1f : 0.3f;
        _canvasGroup.blocksRaycasts = hasTeammate;
    }
}
