using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Popup nhá» hiá»‡n khi chuá»™t pháº£i vÃ o item trong inventory
// Singleton: toÃ n UI chá»‰ cÃ³ 1 popup dÃ¹ng chung cho má»i slot
public class UIItemContextMenu : Singleton<UIItemContextMenu>
{
    [SerializeField] private Button _btnUse;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private UIInventoryManager _uiInventoryManager;

    private int _InventoryIndex;
    private TypeItem _itemType;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBtnUse();
        LoadCanvasRect();
        LoadUIInventoryManager();
    }
    private void LoadBtnUse()
    {
        if (_btnUse != null) return;
        _btnUse = transform.Find("BtnEquip").GetComponent<Button>();
    }

    private void LoadCanvasRect()
    {
        if (_canvasRect != null) return;
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void LoadUIInventoryManager()
    {
        if (_uiInventoryManager != null) return;
        _uiInventoryManager = transform.parent.GetComponentInChildren<UIInventoryManager>();
    }

    protected override void Start()
    {
        base.Start();
        _btnUse.onClick.AddListener(OnClickUse);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        HanleClick();
    }

    private void HanleClick()
    {
        if (!InputManager.Instance.LeftMouseDown) return;
        if (IsPointerOverSelf()) return;
        Hide();
    }


    private bool IsPointerOverSelf()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, Camera.main);
    }
    public void Show(Vector2 screenPos, int inventoryIndex, TypeItem itemType)
    {
        _InventoryIndex = inventoryIndex;
        _itemType = itemType;
        SetPosition(screenPos);
        gameObject.SetActive(true);
    }

    private void SetPosition(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, Camera.main, out Vector2 localPos);
        GetComponent<RectTransform>().anchoredPosition = localPos;
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickUse()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.UIClick);
        if (_itemType == TypeItem.PowerUp)
        {
            var localPlayer = PlayerCtrl.AllPlayers.Find(p => p.PhotonView.IsMine);
            if (localPlayer != null) localPlayer.PlayerPowerUpManager.Use(_InventoryIndex);
            _uiInventoryManager.Refresh();
        }
        else
        {
            UICharacterPanel.Instance.TryEquip(_InventoryIndex);
        }
        Hide();
    }
}
