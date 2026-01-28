using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float smoothTime = 0.6f;
    [SerializeField] private Vector2 verticalLimits = new Vector2(-10f, 40f);
    [SerializeField] private float distance = 7f; 
    [SerializeField] private float height = 2f; 

    private Transform _target;
    private IInputService _inputService;

    private float _currentX = 0f;
    private float _currentY = 0f;
    private Vector3 _currentVelocity; 

    [Inject]
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        HandleInput();
        MoveCamera();
    }

    private void HandleInput()
    {
        Vector2 look = _inputService.LookInput;

        _currentX += look.x * sensitivity;
        _currentY -= look.y * sensitivity;

        _currentY = Mathf.Clamp(_currentY, verticalLimits.x, verticalLimits.y);
    }

    private void MoveCamera()
    {
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);

        Vector3 targetPosition = _target.position + Vector3.up * height - (rotation * Vector3.forward * distance);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);

        transform.LookAt(_target.position + Vector3.up * height);
    }
}