using DG.Tweening;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private RailTileMapSystem _railTileMap;
    [SerializeField] private Transform _detectPoint;
    [SerializeField] private GameContext _gameContext;
    [SerializeField] private float _aetherCostPerMove = 0.1f;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnDuration = 0.5f;
    [SerializeField] private Vector3 _startDirection = Vector3.forward;
    [SerializeField] private float _yawOffsetDegrees;

    private Vector3 _currentDirection;
    private RailTile _currentTile;
    private Tween _moveTween;
    private Tween _turnTween;

    public float Speed => _speed;
    public bool IsMoving => _moveTween != null && _moveTween.IsActive() && _moveTween.IsPlaying();
    public RailTile CurrentTile => _currentTile;

    private void Start()
    {
        _currentDirection = Flatten(_startDirection);
        ApplyRotation(_currentDirection);
        MoveNextTile();
    }

    private void MoveNextTile()
    {
        if (!_railTileMap.TryGetTile(_detectPoint.position, out RailTile currentTile))
        {
            _moveTween?.Kill();
            _turnTween?.Kill();
            return;
        }

        _currentTile = currentTile;

        Vector3 startPosition = currentTile.transform.position;
        startPosition.y = transform.position.y;

        if ((transform.position - startPosition).sqrMagnitude > 0.01f)
        {
            transform.position = startPosition;
        }


        if (!currentTile.TryGetExitDirection(_currentDirection, out Vector3 nextDirection))
        {
            _moveTween?.Kill();
            _turnTween?.Kill();
            return;
        }
        if (currentTile.Type == RailTile.RailType.Curve)
        {
            MoveCurve(nextDirection);
            return;
        }

        _currentDirection = nextDirection;
        MoveStraight();



    }

    private void MoveStraight()
    {
        if (!_gameContext.AetherWallet.TrySpend(_aetherCostPerMove, AetherChangeReason.TrainMove))
        {
            return;
        }

        Vector3 targetPosition = transform.position + _currentDirection * _railTileMap.TileSize;
        float duration = _railTileMap.TileSize / _speed;

        _moveTween = transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(MoveNextTile);
    }

    private void MoveCurve(Vector3 nextDirection)
    {
        if (!_gameContext.AetherWallet.TrySpend(_aetherCostPerMove, AetherChangeReason.TrainMove))
        {
            return;
        }

        _currentDirection = nextDirection;

        Vector3 targetPosition = transform.position + _currentDirection * _railTileMap.TileSize;
        Quaternion targetRotation = Quaternion.LookRotation(_currentDirection, Vector3.up) * Quaternion.Euler(0f, _yawOffsetDegrees, 0f);
        float duration = _railTileMap.TileSize / _speed;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOMove(targetPosition, duration).SetEase(Ease.Linear));
        sequence.Join(transform.DORotateQuaternion(targetRotation, _turnDuration).SetEase(Ease.InOutSine));
        sequence.OnComplete(MoveNextTile);

        _moveTween = sequence;
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
