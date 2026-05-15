using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private ZombieController _zombiePrefab;
    [SerializeField] private GameContext _gameContext;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private int _maxZombieCount = 20;
    [SerializeField] private bool _spawnOnStart = true;

    private int _currentZombieCount;
    private float _spawnTimer;

    private void Start()
    {
        if (_spawnOnStart)
        {
            Spawn();
        }
    }

    private void Update()
    {
        if (_currentZombieCount >= _maxZombieCount)
        {
            return;
        }

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer < _spawnInterval)
        {
            return;
        }

        _spawnTimer = 0f;
        Spawn();
    }

    public void Spawn()
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        ZombieController zombie = Instantiate(_zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        zombie.Init(_gameContext, OnZombieDead);

        _currentZombieCount++;
    }
    private void OnZombieDead(ZombieController zombie)
    {
        _currentZombieCount--;
    }

}
