using System;
using UnityEngine;

[Serializable]
public class ShopItemTradeData
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private int _buyPrice;
    [SerializeField] private int _sellPrice;

    public ItemType ItemType => _itemType;
    public int BuyPrice => _buyPrice;
    public int SellPrice => _sellPrice;
    public bool CanBuy => _buyPrice > 0;
    public bool CanSell => _sellPrice > 0;
}