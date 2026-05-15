using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Monster/Zombie Data")]
public class ZombieData : ScriptableObject
{
    [SerializeField] private float _patrolSpeed = 1.5f;
    [SerializeField] private float _moveToRailSpeed = 2.5f;
    [SerializeField] private float _chaseTrainSpeed = 4f;
    [SerializeField] private float _detectTrainRadius = 12f;
    [SerializeField] private float _detectRailRadius = 10f;
    [SerializeField] private float _wanderRailChangeRadius = 6f;
    [SerializeField] private float _wanderRailChangeChance = 0.35f;
    [SerializeField] private float _arriveDistance = 0.2f;
    [SerializeField] private float _detectInterval = 0.2f;
    [SerializeField] private int _aetherReward = 1;
    [SerializeField] private float _chaseTrainRadius = 5f;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] private float _attackInterval = 1f;


    public float DetectInterval => _detectInterval;
    public int AetherReward => _aetherReward;
    public float PatrolSpeed => _patrolSpeed;
    public float MoveToRailSpeed => _moveToRailSpeed;
    public float ChaseTrainSpeed => _chaseTrainSpeed;
    public float DetectTrainRadius => _detectTrainRadius;
    public float ChaseTrainRadius => _chaseTrainRadius;
    public float AttackDamage => _attackDamage;
    public float AttackInterval => _attackInterval;
    public float DetectRailRadius => _detectRailRadius;
    public float WanderRailChangeRadius => _wanderRailChangeRadius;
    public float WanderRailChangeChance => _wanderRailChangeChance;
    public float ArriveDistance => _arriveDistance;
}
