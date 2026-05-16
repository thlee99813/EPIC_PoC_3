using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseZoneShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _shopNameText;
    [SerializeField] private ItemInventory _inventory;
    [SerializeField] private AetherWallet _aetherWallet;
    [SerializeField] private Transform _rowParent;
    [SerializeField] private ShopItemRowView _rowPrefab;
    [SerializeField] private Button _completeButton;

    private readonly List<ShopItemRowView> _rows = new();
    private Action _closed;

    private void Awake()
    {
        _completeButton.onClick.AddListener(Close);
        _root.SetActive(false);
    }

    private void OnEnable()
    {
        _inventory.Changed += Refresh;
    }

    private void OnDisable()
    {
        _inventory.Changed -= Refresh;
    }

    public void Open(BaseZoneShopData shopData, Action closed)
    {
        _closed = closed;
        _shopNameText.text = shopData.ShopName;

        ClearRows();

        foreach (ShopItemTradeData tradeData in shopData.Trades)
        {
            ShopItemRowView row = Instantiate(_rowPrefab, _rowParent);
            row.Init(this, tradeData);
            _rows.Add(row);
        }

        _root.SetActive(true);
        Refresh();
    }

    public void Buy(ShopItemTradeData tradeData)
    {
        if (!_aetherWallet.TrySpend(tradeData.BuyPrice, AetherChangeReason.BuyItem))
        {
            return;
        }

        _inventory.Add(tradeData.ItemType, 1);
    }

    public void Sell(ShopItemTradeData tradeData)
    {
        if (!_inventory.TryRemove(tradeData.ItemType, 1))
        {
            return;
        }

        _aetherWallet.Add(tradeData.SellPrice, AetherChangeReason.SellItem);
    }

    private void Refresh()
    {
        foreach (ShopItemRowView row in _rows)
        {
            row.Refresh(_inventory);
        }
    }

    private void Close()
    {
        _root.SetActive(false);
        _closed?.Invoke();
        _closed = null;
    }

    private void ClearRows()
    {
        foreach (ShopItemRowView row in _rows)
        {
            Destroy(row.gameObject);
        }

        _rows.Clear();
    }

    private void OnDestroy()
    {
        _completeButton.onClick.RemoveListener(Close);
    }
}