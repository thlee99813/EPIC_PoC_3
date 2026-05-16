using TMPro;
using UnityEngine;

public class TrainRunButtonController : MonoBehaviour
{
    [SerializeField] private TrainController _trainController;
    [SerializeField] private BaseZoneInteractionController _baseZoneInteractionController;
    [SerializeField] private TMP_Text _buttonText;

    public void OnClick()
    {
        _trainController.ToggleRun();
        Refresh();

        if (_trainController.IsStopped)
        {
            _baseZoneInteractionController.TryOpenNearestZone();
        }
    }

    private void Refresh()
    {
        _buttonText.text = _trainController.IsStopped ? "출발" : "정지";
    }
}