using DG.Tweening;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private RailTileMapSystem _railTileMap;
    
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnDuration = 0.5f;
    [SerializeField] private Vector3 _startDirection = Vector3.forward;
    [SerializeField] private float _yawOffsetDegrees;

    private Vector3 _currentDirection;
    private Tween _moveTween;
    private Tween _turnTween;
    public float Speed => _speed;

    private void Start()
    {
        _currentDirection = Flatten(_startDirection);
        ApplyRotation(_currentDirection);
        MoveNextTile();
    }

    private void MoveNextTile()
    {
        RailTile currentTile = _railTileMap.GetTile(transform.position);
        Vector3 startPosition = _railTileMap.GetTileCenter(transform.position);
        transform.position = startPosition;

        if (currentTile.Type == RailTile.RailType.Curve)
        {
            TurnOnCurve(currentTile);
            return;
        }

        MoveStraight();
    }

    private void MoveStraight()
    {
        Vector3 targetPosition = transform.position + _currentDirection * _railTileMap.TileSize;
        float duration = _railTileMap.TileSize / _speed;

        _moveTween = transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(MoveNextTile);
    }

    private void TurnOnCurve(RailTile railTile)
    {
        int turnDirection = railTile.GetTurnDirection(_currentDirection);
        _currentDirection = Quaternion.Euler(0f, 90f * turnDirection, 0f) * _currentDirection;

        Quaternion targetRotation = Quaternion.LookRotation(_currentDirection, Vector3.up) * Quaternion.Euler(0f, _yawOffsetDegrees, 0f);

        _turnTween = transform.DORotateQuaternion(targetRotation, _turnDuration).SetEase(Ease.InOutSine).OnComplete(MoveStraight);
    }

    private void ApplyRotation(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(0f, _yawOffsetDegrees, 0f);
    }

    private Vector3 Flatten(Vector3 direction)
    {
        direction.y = 0f;
        return direction.normalized;
    }

    private void OnDestroy()
    {
        _moveTween?.Kill();
        _turnTween?.Kill();
    }
}
