using TMPro;
using UnityEngine;

public class AetherTextView : MonoBehaviour
{
    [SerializeField] private AetherWallet _wallet;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private string _format = "남은 에테르 : {0}";

    private void OnEnable()
    {
        _wallet.Changed += Refresh;
        Refresh(_wallet.CurrentAmount);
    }

    private void OnDisable()
    {
        _wallet.Changed -= Refresh;
    }

    private void Refresh(float amount)
    {
        _amountText.text = string.Format(_format, amount);
    }
}