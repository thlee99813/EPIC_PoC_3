using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Base Zone/Base Zone Shop")]
public class BaseZoneShopData : ScriptableObject
{
    [SerializeField] private string _shopName;
    [SerializeField] private ShopItemTradeData[] _trades;

    public string ShopName => _shopName;
    public ShopItemTradeData[] Trades => _trades;
}