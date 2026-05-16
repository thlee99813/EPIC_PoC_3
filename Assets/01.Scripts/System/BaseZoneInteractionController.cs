using UnityEngine;

public class BaseZoneInteractionController : MonoBehaviour
{
    [SerializeField] private GameContext _gameContext;
    [SerializeField] private GameStateController _gameStateController;
    [SerializeField] private BaseZoneDialoguePanel _dialoguePanel;
    [SerializeField] private BaseZoneShopPanel _shopPanel;
    [SerializeField] private BaseZone[] _zones;

    private BaseZone _currentZone;

    public bool TryOpenNearestZone()
    {
        if (_gameContext.TrainController.IsMoving)
        {
            return false;
        }

        BaseZone nearestZone = GetNearestZone();

        if (nearestZone == null)
        {
            return false;
        }

        _gameStateController.SetState(GameState.Event);
        _currentZone = nearestZone;
        _dialoguePanel.Open(nearestZone.DialogueData, OnDialogueClosed);
        return true;
    }

    private BaseZone GetNearestZone()
    {
        Vector3 trainPosition = _gameContext.Train.position;
        BaseZone nearestZone = null;
        float nearestDistance = float.MaxValue;

        foreach (BaseZone zone in _zones)
        {
            if (!zone.Contains(trainPosition))
            {
                continue;
            }

            float distance = (zone.transform.position - trainPosition).sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestZone = zone;
            }
        }

        return nearestZone;
    }

    private void OnDialogueClosed()
    {
        switch (_currentZone.ResultType)
        {
            case BaseZoneResultType.Shop:
                _shopPanel.Open(_currentZone.ShopData, OnZoneClosed);
                break;

            case BaseZoneResultType.GameClear:
                _currentZone = null;
                _gameStateController.GameClear();
                break;

            case BaseZoneResultType.None:
                OnZoneClosed();
                break;
        }
    }

    private void OnZoneClosed()
    {
        _currentZone = null;
        _gameStateController.SetState(GameState.Playing);
    }
}