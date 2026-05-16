using TMPro;
using UnityEngine;

public class TrainHealthTextView : MonoBehaviour
{
    [SerializeField] private TrainHealth _trainHealth;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private string _format = "기차 남은피 : {0:0}";

    private void OnEnable()
    {
        _trainHealth.Changed += Refresh;
        Refresh(_trainHealth.CurrentHealth, _trainHealth.MaxHealth);
    }

    private void OnDisable()
    {
        _trainHealth.Changed -= Refresh;
    }

    private void Refresh(float currentHealth, float maxHealth)
    {
        _healthText.text = string.Format(_format, currentHealth);
    }
}