using UnityEngine;

public class ChestModel : SaiMonoBehaviour
{
    [SerializeField] private Transform _chestClosing;
    [SerializeField] private Transform _chestOpening;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadChestClosing();
        LoadChestOpening();
    }

    private void LoadChestClosing()
    {
        if (_chestClosing != null) return;
        _chestClosing = transform.Find("ChestClosing");
        Debug.Log(transform.name + ": Load ChestClosing", gameObject);
    }

    private void LoadChestOpening()
    {
        if (_chestOpening != null) return;
        _chestOpening = transform.Find("ChestOpening");
        Debug.Log(transform.name + ": Load ChestOpening", gameObject);
    }

    public void PlayOpen()
    {
        if (_chestClosing != null) _chestClosing.gameObject.SetActive(false);
        if (_chestOpening != null) _chestOpening.gameObject.SetActive(true);
    }

    public void PlayClose()
    {
        if (_chestClosing != null) _chestClosing.gameObject.SetActive(true);
        if (_chestOpening != null) _chestOpening.gameObject.SetActive(false);
    }
}
