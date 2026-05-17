using System;
using UnityEngine;

public class NPCUpgradeInteract : BaseInteract
{

    protected override void Interact()
    {
        if (!_playerInRange) return;
        if (InputManager.Instance.GetMouseIteract())
        {
            CenterCtrl.Instance.OpenUpgrade();
        }
    }

}
