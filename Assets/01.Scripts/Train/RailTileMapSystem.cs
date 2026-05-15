using System.Collections.Generic;
using UnityEngine;

public class RailTileMapSystem : MonoBehaviour
{
    [SerializeField] private Transform _railRoot;
    [SerializeField] private float _tileSize = 5.45f;

    private readonly Dictionary<Vector2Int, RailTile> _tiles = new();

    public float TileSize => _tileSize;

    private void Awake()
    {
        Refresh();
    }

    public RailTile GetTile(Vector3 worldPosition)
    {
        _tiles.TryGetValue(WorldToGrid(worldPosition), out RailTile railTile);
        return railTile;
    }

    public Vector3 GetTileCenter(Vector3 worldPosition)
    {
        RailTile railTile = GetTile(worldPosition);
        Vector3 position = railTile.transform.position;
        position.y = worldPosition.y;
        return position;
    }
    public RailTile GetNearestTile(Vector3 worldPosition, float maxDistance)
    {
        RailTile nearestTile = null;
        float nearestSqrDistance = maxDistance * maxDistance;

        foreach (RailTile railTile in _tiles.Values)
        {
            float sqrDistance = (railTile.transform.position - worldPosition).sqrMagnitude;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearestTile = railTile;
            }
        }

        return nearestTile;
    }


    private Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / _tileSize),
            Mathf.RoundToInt(worldPosition.z / _tileSize)
        );
    }
    public void Refresh()
    {
        RailTile[] railTiles = _railRoot.GetComponentsInChildren<RailTile>();

        foreach (RailTile railTile in railTiles)
        {
            Vector2Int gridPosition = WorldToGrid(railTile.transform.position);
            _tiles[gridPosition] = railTile;
        }
    }
}
