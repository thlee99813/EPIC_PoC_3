using System.Collections.Generic;
using UnityEngine;

public class RailTileMapSystem : MonoBehaviour
{
    [SerializeField] private Transform _railRoot;
    [SerializeField] private Transform _gridOrigin;
    [SerializeField] private float _tileSize = 5.45f;

    private readonly Dictionary<Vector2Int, RailTile> _tiles = new();

    public float TileSize => _tileSize;

    private void Awake()
    {
        Refresh();
    }

    public bool TryGetTile(Vector3 worldPosition, out RailTile railTile)
    {
        Vector2Int gridPosition = WorldToGrid(worldPosition);

        if (!_tiles.TryGetValue(gridPosition, out railTile) || railTile == null)
        {
            return false;
        }

        Vector3 railPosition = railTile.transform.position;
        Vector2 railXZ = new Vector2(railPosition.x, railPosition.z);
        Vector2 worldXZ = new Vector2(worldPosition.x, worldPosition.z);

        if ((railXZ - worldXZ).sqrMagnitude > 1.5f * 1.5f)
        {
            Debug.LogError($"Tile Mismatch / World:{worldPosition}, Grid:{gridPosition}, Rail:{railTile.name}, RailPos:{railTile.transform.position}, Parent:{railTile.transform.parent.name}");
            railTile = null;
            return false;
        }

        return true;
    }


    public bool TryGetTileCenter(Vector3 worldPosition, out Vector3 centerPosition)
    {
        if (!TryGetTile(worldPosition, out RailTile railTile))
        {
            centerPosition = worldPosition;
            return false;
        }

        centerPosition = railTile.transform.position;
        centerPosition.y = worldPosition.y;
        return true;
    }

    public Vector3 GetTileCenter(Vector3 worldPosition)
    {
        if (TryGetTileCenter(worldPosition, out Vector3 centerPosition))
        {
            return centerPosition;
        }

        return worldPosition;
    }

    public RailTile GetNearestTile(Vector3 worldPosition, float maxDistance)
    {
        RailTile nearestTile = null;
        float nearestSqrDistance = maxDistance * maxDistance;

        foreach (RailTile railTile in _tiles.Values)
        {
            if (railTile == null)
            {
                continue;
            }

            float sqrDistance = (railTile.transform.position - worldPosition).sqrMagnitude;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearestTile = railTile;
            }
        }

        return nearestTile;
    }
public RailTile GetRandomTileInRadius(Vector3 worldPosition, float radius)
{
    List<RailTile> candidates = new List<RailTile>();
    float radiusSqr = radius * radius;

    foreach (RailTile railTile in _tiles.Values)
    {
        if (railTile == null)
        {
            continue;
        }

        float sqrDistance = (railTile.transform.position - worldPosition).sqrMagnitude;

        if (sqrDistance <= radiusSqr)
        {
            candidates.Add(railTile);
        }
    }

    if (candidates.Count == 0)
    {
        return null;
    }

    return candidates[Random.Range(0, candidates.Count)];
}

    private Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - _gridOrigin.position;

        return new Vector2Int(
            Mathf.RoundToInt(localPosition.x / _tileSize),
            Mathf.RoundToInt(localPosition.z / _tileSize)
        );
    }

    public void Refresh()
    {
        _tiles.Clear();

        RailTile[] railTiles = _railRoot.GetComponentsInChildren<RailTile>();

        foreach (RailTile railTile in railTiles)
        {
            Vector2Int gridPosition = WorldToGrid(railTile.transform.position);

            if (_tiles.TryGetValue(gridPosition, out RailTile existingTile))
            {
                Debug.LogError($"Duplicate Rail Grid / Grid:{gridPosition}, Existing:{existingTile.name}, ExistingPos:{existingTile.transform.position}, New:{railTile.name}, NewPos:{railTile.transform.position}");
            }

            _tiles[gridPosition] = railTile;
        }
    }

}
