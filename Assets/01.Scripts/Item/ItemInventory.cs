using System;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    [SerializeField] private int _startWater;
    [SerializeField] private int _startBread;

    private int _water;
    private int _bread;

    public event Action Changed;

    private void Awake()
    {
        _water = _startWater;
        _bread = _startBread;
    }

    private void Start()
    {
        Changed?.Invoke();
    }

    public int GetAmount(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Water => _water,
            ItemType.Bread => _bread,
            _ => 0
        };
    }

    public void Add(ItemType itemType, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        SetAmount(itemType, GetAmount(itemType) + amount);
    }

    public bool TryRemove(ItemType itemType, int amount)
    {
        if (amount <= 0)
        {
            return true;
        }

        int currentAmount = GetAmount(itemType);

        if (currentAmount < amount)
        {
            return false;
        }

        SetAmount(itemType, currentAmount - amount);
        return true;
    }

    private void SetAmount(ItemType itemType, int amount)
    {
        switch (itemType)
        {
            case ItemType.Water:
                _water = amount;
                break;

            case ItemType.Bread:
                _bread = amount;
                break;
        }

        Changed?.Invoke();
    }
}