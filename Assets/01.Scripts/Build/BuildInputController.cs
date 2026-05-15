using UnityEngine;
using UnityEngine.InputSystem;

public class BuildInputController : MonoBehaviour
{
    [SerializeField] private RailBuildController _railBuildController;
    [SerializeField] private BuildModeUI _buildModeUI;
    [SerializeField] private RailTileMapSystem _railTileMapSystem;


    private bool _isBuildEnabled;

    private void Start()
    {
        SetBuildEnabled(false);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;

        if (keyboard.bKey.wasPressedThisFrame)
        {
            SetBuildEnabled(!_isBuildEnabled);
        }

        if (!_isBuildEnabled)
        {
            return;
        }

        _railBuildController.SetPointerScreenPosition(mouse.position.ReadValue());

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            SetMode(BuildMode.RemoveRail);
        }

        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            SetMode(BuildMode.StraightRail);
        }

        if (keyboard.digit3Key.wasPressedThisFrame)
        {
            SetMode(BuildMode.CurveRail);
        }

        if (keyboard.qKey.wasPressedThisFrame)
        {
            _railBuildController.RotatePreview(-90f);
        }

        if (keyboard.eKey.wasPressedThisFrame)
        {
            _railBuildController.RotatePreview(90f);
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            _railBuildController.TryBuild();
        }
    }

    private void SetBuildEnabled(bool isEnabled)
    {
        _isBuildEnabled = isEnabled;
        _railTileMapSystem.Refresh();
        Time.timeScale = isEnabled ? 0f : 1f;

        _railBuildController.SetBuildEnabled(isEnabled);
        _buildModeUI.SetVisible(isEnabled);

        if (isEnabled)
        {
            SetMode(BuildMode.RemoveRail);
        }
    }

    private void SetMode(BuildMode mode)
    {
        _railBuildController.SetMode(mode);
        _buildModeUI.SetMode(mode);
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
