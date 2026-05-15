using UnityEngine;

[CreateAssetMenu(menuName = "Epic/Monster/Zombie Data")]
public class ZombieData : ScriptableObject
{
    [SerializeField] private float _patrolSpeed = 1.5f;
    [SerializeField] private float _moveToRailSpeed = 2.5f;
    [SerializeField] private float _detectTrainRadius = 12f;
    [SerializeField] private float _detectRailRadius = 10f;
    [SerializeField] private float _arriveDistance = 0.2f;
    [SerializeField] private float _detectInterval = 0.2f;
    public float DetectInterval => _detectInterval;
    public float PatrolSpeed => _patrolSpeed;
    public float MoveToRailSpeed => _moveToRailSpeed;
    public float DetectTrainRadius => _detectTrainRadius;
    public float DetectRailRadius => _detectRailRadius;
    public float ArriveDistance => _arriveDistance;
}
