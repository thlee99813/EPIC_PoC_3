using UnityEngine;

public class RailBuildController : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _tileLayerMask;

    [Header("Rail Prefabs")]
    [SerializeField] private GameObject _straightRailPrefab;
    [SerializeField] private GameObject _curveRailPrefab;

    [Header("Preview Prefabs")]
    [SerializeField] private GameObject _straightRailPreviewPrefab;
    [SerializeField] private GameObject _curveRailPreviewPrefab;
    [SerializeField] private GameObject _straightRailDeclinePreviewPrefab;
    [SerializeField] private GameObject _curveRailDeclinePreviewPrefab;
    [SerializeField] private GameObject _removePrefab;

    private BuildMode _currentMode = BuildMode.RemoveRail;
    private BuildTile _currentTile;
    private GameObject _previewObject;
    private GameObject _currentPreviewPrefab;
    private Vector2 _pointerScreenPosition;
    private float _previewYRotation;
    private bool _isBuildEnabled;

    private void Update()
    {
        if (!_isBuildEnabled)
        {
            return;
        }

        UpdateHoverTile();
        UpdatePreview();
    }

    public void SetBuildEnabled(bool isEnabled)
    {
        _isBuildEnabled = isEnabled;

        if (!isEnabled)
        {
            _currentTile = null;
            DestroyPreview();
        }
    }

    public void SetMode(BuildMode mode)
    {
        _currentMode = mode;
        DestroyPreview();
    }

    public void SetPointerScreenPosition(Vector2 pointerScreenPosition)
    {
        _pointerScreenPosition = pointerScreenPosition;
    }

    public void RotatePreview(float angle)
    {
        if (!_isBuildEnabled || _currentMode == BuildMode.RemoveRail)
        {
            return;
        }

        _previewYRotation += angle;

        if (_previewYRotation > 180f)
        {
            _previewYRotation -= 360f;
        }

        if (_previewYRotation < -180f)
        {
            _previewYRotation += 360f;
        }
    }

    public void TryBuild()
    {
        if (!_isBuildEnabled || _currentTile == null)
        {
            return;
        }

        if (_currentMode == BuildMode.RemoveRail)
        {
            _currentTile.DestroyRail();
            DestroyPreview();
            return;
        }

        if (_currentTile.HasRail)
        {
            return;
        }

        GameObject railPrefab = GetRailPrefab();
        GameObject rail = Instantiate(railPrefab, _currentTile.BuildParent);
        rail.transform.localPosition = Vector3.zero;
        rail.transform.localRotation = Quaternion.Euler(0f, _previewYRotation, 0f);

        _currentTile.SetRail(rail);
        DestroyPreview();
    }

    private void UpdateHoverTile()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_pointerScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _tileLayerMask))
        {
            _currentTile = hit.collider.GetComponentInParent<BuildTile>();
            return;
        }

        _currentTile = null;
    }

    private void UpdatePreview()
    {
        if (_currentTile == null)
        {
            DestroyPreview();
            return;
        }

        GameObject previewPrefab = GetPreviewPrefab();

        if (_previewObject == null || _currentPreviewPrefab != previewPrefab)
        {
            DestroyPreview();
            _currentPreviewPrefab = previewPrefab;
            _previewObject = Instantiate(previewPrefab, _currentTile.BuildParent);
        }

        if (_previewObject.transform.parent != _currentTile.BuildParent)
        {
            _previewObject.transform.SetParent(_currentTile.BuildParent, false);
        }

        _previewObject.transform.localPosition = Vector3.zero;
        _previewObject.transform.localRotation = Quaternion.Euler(0f, _previewYRotation, 0f);
    }

    private GameObject GetRailPrefab()
    {
        return _currentMode == BuildMode.StraightRail ? _straightRailPrefab : _curveRailPrefab;
    }

    private GameObject GetPreviewPrefab()
    {
        if (_currentMode == BuildMode.StraightRail)
        {
            return _currentTile.HasRail ? _straightRailDeclinePreviewPrefab : _straightRailPreviewPrefab;
        }
        else if (_currentMode == BuildMode.CurveRail)
        {
            return _currentTile.HasRail ? _curveRailDeclinePreviewPrefab : _curveRailPreviewPrefab;
        }
        return _removePrefab;
    }

    private void DestroyPreview()
    {
        if (_previewObject == null)
        {
            return;
        }

        Destroy(_previewObject);
        _previewObject = null;
        _currentPreviewPrefab = null;
    }
}
