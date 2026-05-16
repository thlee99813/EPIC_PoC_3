using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemRowView : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private TMP_Text _buyPriceText;
    [SerializeField] private TMP_Text _sellPriceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private BaseZoneShopPanel _shopPanel;
    private ShopItemTradeData _tradeData;

    public void Init(BaseZoneShopPanel shopPanel, ShopItemTradeData tradeData)
    {
        _shopPanel = shopPanel;
        _tradeData = tradeData;

        _buyButton.onClick.AddListener(Buy);
        _sellButton.onClick.AddListener(Sell);

        _itemNameText.text = GetItemName(tradeData.ItemType);
        _buyPriceText.text = tradeData.CanBuy ? $"{tradeData.BuyPrice}" : "-";
        _sellPriceText.text = tradeData.CanSell ? $"{tradeData.SellPrice}" : "-";
        _buyButton.gameObject.SetActive(tradeData.CanBuy);
        _sellButton.gameObject.SetActive(tradeData.CanSell);
    }

    public void Refresh(ItemInventory inventory)
    {
        _amountText.text = $"보유 {inventory.GetAmount(_tradeData.ItemType)}";
    }

    private void Buy()
    {
        _shopPanel.Buy(_tradeData);
    }

    private void Sell()
    {
        _shopPanel.Sell(_tradeData);
    }

    private string GetItemName(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Water => "물",
            ItemType.Bread => "빵",
            _ => itemType.ToString()
        };
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(Buy);
        _sellButton.onClick.RemoveListener(Sell);
    }
}