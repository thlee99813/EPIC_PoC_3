using UnityEngine;
using System;

public class ZombieController : MonoBehaviour
{
    private enum ZombieState
    {
        Patrol,
        MoveToRail,
        WanderOnRail,
        ChaseTrain,
        Dead
    }

    [SerializeField] private ZombieData _data;
    [SerializeField] private float _killTrainSpeed = 3f;

    private GameContext _gameContext;
    private ZombieState _state;
    private Vector3 _patrolTarget;
    private RailTile _targetRailTile;
    private Action<ZombieController> _onDead;
    private float _nextAttackTime;


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
            case ZombieState.ChaseTrain:
                UpdateChaseTrain();
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
        if (_state == ZombieState.Dead || _state == ZombieState.ChaseTrain)
        {
            return;
        }

        if (_state == ZombieState.WanderOnRail && IsTrainChaseable())
        {
            _state = ZombieState.ChaseTrain;
            return;
        }

        if (_state != ZombieState.Patrol)
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
        if (!IsTargetRailValid())
        {
            ResetToPatrol();
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

    private bool IsTargetRailValid()
    {
        return _targetRailTile != null;
    }

    private void ResetToPatrol()
    {
        _targetRailTile = null;
        _state = ZombieState.Patrol;
        SetRandomPatrolTarget();
    }

    private void UpdateWanderOnRail()
    {
        if (!IsTargetRailValid())
        {
            ResetToPatrol();
            return;
        }

        MoveTo(_patrolTarget, _data.PatrolSpeed);

        if (Vector3.Distance(transform.position, _patrolTarget) <= _data.ArriveDistance)
        {
            SetRailWanderTarget();
        }
    }
    private void UpdateChaseTrain()
    {
        if (!IsTrainDetected())
        {
            SetRailWanderTarget();
            _state = ZombieState.WanderOnRail;
            return;
        }

        Vector3 targetPosition = _gameContext.Train.position;
        targetPosition.y = transform.position.y;

        MoveTo(targetPosition, _data.ChaseTrainSpeed);
    }

    private void SetRailWanderTarget()
    {
        if (!IsTargetRailValid())
        {
            ResetToPatrol();
            return;
        }

        if (UnityEngine.Random.value <= _data.WanderRailChangeChance)
        {
            RailTile nextRailTile = _gameContext.RailTileMap.GetRandomTileInRadius(_targetRailTile.transform.position, _data.WanderRailChangeRadius);

            if (nextRailTile != null)
            {
                _targetRailTile = nextRailTile;
            }
        }

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

    private bool IsTrainChaseable()
    {
        return Vector3.Distance(transform.position, _gameContext.Train.position) <= _data.ChaseTrainRadius;
    }

    private void SetRandomPatrolTarget()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 10f;
        _patrolTarget = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTrainContact(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleTrainContact(other);
    }

    private void HandleTrainContact(Collider other)
    {
        TrainController trainController = other.GetComponentInParent<TrainController>();

        if (trainController == null)
        {
            return;
        }

        if (trainController.IsMoving && trainController.Speed >= _killTrainSpeed)
        {
            Die();
            return;
        }

        TryAttackTrain(other);
    }

    private void TryAttackTrain(Collider other)
    {
        if (Time.time < _nextAttackTime)
        {
            return;
        }

        TrainHealth trainHealth = other.GetComponentInParent<TrainHealth>();
        trainHealth.TakeDamage(_data.AttackDamage);

        _nextAttackTime = Time.time + _data.AttackInterval;
    }

    private void Die()
    {
        _state = ZombieState.Dead;
        _gameContext.AetherWallet.Add(_data.AetherReward, AetherChangeReason.ZombieKill);
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
