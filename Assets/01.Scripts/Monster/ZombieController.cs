using UnityEngine;
using System;

public class ZombieController : MonoBehaviour
{
    private enum ZombieState
    {
        Patrol,
        MoveToRail,
        WanderOnRail,
        Dead
    }

    [SerializeField] private ZombieData _data;
    [SerializeField] private float _killTrainSpeed = 3f;

    private GameContext _gameContext;
    private ZombieState _state;
    private Vector3 _patrolTarget;
    private RailTile _targetRailTile;
    private Action<ZombieController> _onDead;

    public void Init(GameContext gameContext, Action<ZombieController> onDead)
    {
        _gameContext = gameContext;
        _onDead = onDead;
        _state = ZombieState.Patrol;
        SetRandomPatrolTarget();

        _gameContext.ZombieTickSystem.Register(this);
    }


    private void Update()
    {
        if (_state == ZombieState.Dead)
        {
            return;
        }

        switch (_state)
        {
            case ZombieState.Patrol:
                UpdatePatrol();
                break;

            case ZombieState.MoveToRail:
                UpdateMoveToRail();
                break;

            case ZombieState.WanderOnRail:
                UpdateWanderOnRail();
                break;
        }
    }

    private void UpdatePatrol()
    {
        MoveTo(_patrolTarget, _data.PatrolSpeed);

        if (Vector3.Distance(transform.position, _patrolTarget) <= _data.ArriveDistance)
        {
            SetRandomPatrolTarget();
        }
    }
    public void TickDetect()
    {
        if (_state != ZombieState.Patrol)
        {
            return;
        }

        if (!IsTrainDetected())
        {
            return;
        }

        RailTile nearestRail = _gameContext.RailTileMap.GetNearestTile(transform.position, _data.DetectRailRadius);

        if (nearestRail != null)
        {
            _targetRailTile = nearestRail;
            _state = ZombieState.MoveToRail;
        }
    }



    private void UpdateMoveToRail()
    {
        if (_targetRailTile == null)
        {
            _state = ZombieState.Patrol;
            SetRandomPatrolTarget();
            return;
        }

        Vector3 targetPosition = _targetRailTile.transform.position;
        targetPosition.y = transform.position.y;

        MoveTo(targetPosition, _data.MoveToRailSpeed);

        if (Vector3.Distance(transform.position, targetPosition) <= _data.ArriveDistance)
        {
            SetRailWanderTarget();
            _state = ZombieState.WanderOnRail;
        }
    }


    private void UpdateWanderOnRail()
    {
        if (_targetRailTile == null)
        {
            _state = ZombieState.Patrol;
            SetRandomPatrolTarget();
            return;
        }

        MoveTo(_patrolTarget, _data.PatrolSpeed);

        if (Vector3.Distance(transform.position, _patrolTarget) <= _data.ArriveDistance)
        {
            SetRailWanderTarget();
        }
    }

    private void SetRailWanderTarget()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 1.5f;
        Vector3 railPosition = _targetRailTile.transform.position;
        _patrolTarget = new Vector3(railPosition.x + randomCircle.x, transform.position.y, railPosition.z + randomCircle.y);
    }


    private void MoveTo(Vector3 targetPosition, float speed)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }

    private bool IsTrainDetected()
    {
        return Vector3.Distance(transform.position, _gameContext.Train.position) <= _data.DetectTrainRadius;
    }

    private void SetRandomPatrolTarget()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 10f;
        _patrolTarget = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        TrainController trainController = other.GetComponentInParent<TrainController>();

        if (trainController == null)
        {
            return;
        }

        if (trainController.Speed >= _killTrainSpeed)
        {
            Die();
        }
    }

    private void Die()
    {
        _state = ZombieState.Dead;
        _gameContext.ZombieTickSystem.Unregister(this);
        _onDead?.Invoke(this);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (_gameContext == null)
        {
            return;
        }

        _gameContext.ZombieTickSystem.Unregister(this);
    }



}
