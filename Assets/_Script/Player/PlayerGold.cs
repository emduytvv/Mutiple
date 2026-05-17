using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGold : SaiMonoBehaviour
{
    [SerializeField] protected int maxGold = 100000;
    [SerializeField] protected int currentGold = 0;
    public int CurrentGold => currentGold;
    [SerializeField] protected int goldPerTime = 10;
    [SerializeField] protected int timeForAddGold = 2;
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(AddGoldPerTime), timeForAddGold, timeForAddGold);
    }
    public void AddGold(int amount)
    {
        if (amount < 0) return;
        if (currentGold > maxGold - amount) return;
        currentGold += amount;
        CheckMaxGold();
    }
    protected virtual void CheckMaxGold()
    {
        if (currentGold > maxGold) { currentGold = maxGold; }
        if (currentGold < 0) { currentGold = 0; }
    }
    public bool TrySpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            return true;
        }
        else { return false; }
    }
    protected virtual void AddGoldPerTime()
    {
        AddGold(goldPerTime);
    }
}
