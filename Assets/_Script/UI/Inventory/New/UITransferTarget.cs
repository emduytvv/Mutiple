using UnityEngine;
using UnityEngine.UI;

// Drop zone mÃ u vÃ ng trong inventory panel â€” kÃ©o item vÃ o Ä‘Ã¢y Ä‘á»ƒ chuyá»ƒn cho teammate
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
    }

    private void FixedUpdate()
    {
        bool hasTeammate = PlayerCtrl.AllPlayers.Count > 1;
        _canvasGroup.alpha = hasTeammate ? 1f : 0.3f;
        _canvasGroup.blocksRaycasts = hasTeammate;
    }
}
