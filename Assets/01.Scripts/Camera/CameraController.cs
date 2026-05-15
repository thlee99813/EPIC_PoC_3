using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _edgeSize = 80f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 8f;
    [SerializeField] private float _maxZoom = 35f;
    [SerializeField] private float _dragMoveSpeed = 0.05f;


    [SerializeField] private Transform _cameraStartPos;

    [SerializeField] private bool _mouseLock = false;
    private bool _isInitialized = false;


    private void OnEnable()
    {
        if(_mouseLock)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private void LateUpdate()
    {
        if (_isInitialized)
        {
            return;
        }

        _cinemachineCamera.transform.position = _cameraStartPos.position;
        _isInitialized = true;
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null)
        {
            return;
        }

        Vector2 mousePosition = mouse.position.ReadValue();

        if (mouse.rightButton.isPressed)
        {
            MoveByRightMouseDrag(mouse.delta.ReadValue());
        }
        else
        {
            if(_mouseLock)
            {
                MoveByScreenEdge(mousePosition);
            }
        }

        Zoom(mouse.scroll.ReadValue().y);

    }

    private void MoveByScreenEdge(Vector2 mousePosition)
    {
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x <= _edgeSize)
        {
            moveDirection += Vector3.left;
        }

        if (mousePosition.x >= Screen.width - _edgeSize)
        {
            moveDirection += Vector3.right;
        }

        if (mousePosition.y <= _edgeSize)
        {
            moveDirection += Vector3.back;
        }

        if (mousePosition.y >= Screen.height - _edgeSize)
        {
            moveDirection += Vector3.forward;
        }

        _cinemachineCamera.transform.position += moveDirection.normalized * _moveSpeed * Time.unscaledDeltaTime;
    }
    private void MoveByRightMouseDrag(Vector2 mouseDelta)
    {
        if (mouseDelta.sqrMagnitude < 0.01f)
        {
            return;
        }

        Vector3 moveDirection = new Vector3(-mouseDelta.x, 0f, -mouseDelta.y);
        _cinemachineCamera.transform.position += moveDirection * _dragMoveSpeed;
    }


    private void Zoom(float scrollY)
    {
        if (Mathf.Abs(scrollY) < 0.01f)
        {
            return;
        }

        LensSettings lens = _cinemachineCamera.Lens;
        lens.OrthographicSize = Mathf.Clamp(lens.OrthographicSize - scrollY * _zoomSpeed, _minZoom, _maxZoom);
        _cinemachineCamera.Lens = lens;
    }
}
