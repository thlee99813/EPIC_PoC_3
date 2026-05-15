using UnityEngine;
using UnityEngine.UI;

public class BuildModeUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private Image[] _slotBorders;

    private readonly Color _defaultSlotColor = new Color(1f, 1f, 1f, 0.9f);
    private readonly Color _selectedSlotColor = new Color(0f, 1f, 0.8745f, 0.9f);

    public void SetVisible(bool isVisible)
    {
        _root.SetActive(isVisible);
    }

    public void SetMode(BuildMode mode)
    {
        for (int i = 0; i < _slotBorders.Length; i++)
        {
            _slotBorders[i].color = _defaultSlotColor;
        }

        _slotBorders[(int)mode].color = _selectedSlotColor;
    }
}
