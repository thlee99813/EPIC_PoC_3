using System;
using UnityEngine;

public class TrainHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private GameStateController _gameStateController;

    public float _currentHealth;
    private bool _isDead;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public event Action<float, float> Changed;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        Changed?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (_isDead || damage <= 0f)
        {
            return;
        }

        _currentHealth = Mathf.Max(0f, _currentHealth - damage);
        Changed?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0f)
        {
            _isDead = true;
            _gameStateController.GameOver();
        }
    }
}