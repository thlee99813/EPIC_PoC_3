using System;
using UnityEngine;

public class AetherWallet : MonoBehaviour
{
    [SerializeField] private float _startAmount = 40f;
    [SerializeField] private float _maxAmount = 9999f;

    private float _currentAmount;

    public float CurrentAmount => _currentAmount;
    public event Action<float> Changed;

    private void Awake()
    {
        _currentAmount = _startAmount;
    }

    private void Start()
    {
        Changed?.Invoke(_currentAmount);
    }

    public void Add(float amount, AetherChangeReason reason)
    {
        if (amount <= 0)
        {
            return;
        }

        _currentAmount = Mathf.Min(_currentAmount + amount, _maxAmount);
        Changed?.Invoke(_currentAmount);
    }

    public bool CanSpend(float amount)
    {
        return amount <= 0 || _currentAmount >= amount;
    }

    public bool TrySpend(float amount, AetherChangeReason reason)
    {
        if (amount <= 0)
        {
            return true;
        }

        if (_currentAmount < amount)
        {
            return false;
        }

        _currentAmount -= amount;
        Changed?.Invoke(_currentAmount);
        return true;
    }
}