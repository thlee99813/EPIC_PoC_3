using UnityEngine;

public class BuildTile : MonoBehaviour
{
    [SerializeField] private Transform _buildPoint;

    private GameObject _builtRail;

    public Vector3 BuildPosition => _buildPoint.position;
    public Transform BuildParent => transform;
    public bool HasRail => _builtRail != null;

    private void Awake()
    {
        RailTile railTile = GetComponentInChildren<RailTile>();

        if (railTile != null)
        {
            _builtRail = railTile.gameObject;
        }
    }

    public void SetRail(GameObject rail)
    {
        _builtRail = rail;
    }
    public bool TryGetRailTile(out RailTile railTile)
    {
        railTile = _builtRail.GetComponent<RailTile>();
        return railTile != null;
    }

    public void DestroyRail()
    {
        if (_builtRail == null)
        {
            return;
        }

        Destroy(_builtRail);
        _builtRail = null;
    }
}
