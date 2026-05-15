using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private Transform _train;
    [SerializeField] private TrainController _trainController;
    [SerializeField] private RailTileMapSystem _railTileMap;
    [SerializeField] private ZombieTickSystem _zombieTickSystem;
    [SerializeField] private AetherWallet _aetherWallet;

    public Transform Train => _train;
    public TrainController TrainController => _trainController;
    public RailTileMapSystem RailTileMap => _railTileMap;
    public ZombieTickSystem ZombieTickSystem => _zombieTickSystem;
    public AetherWallet AetherWallet => _aetherWallet;
}
