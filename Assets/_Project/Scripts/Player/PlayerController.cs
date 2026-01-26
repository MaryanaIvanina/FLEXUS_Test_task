using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController _characterController;
    private IInputService _inputService;
    private Transform _cameraTransform;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }

    private void OnEnable() => _inputService.Enable();
    private void OnDisable() => _inputService.Disable();

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = _inputService.MoveInput;

        if (input == Vector2.zero) return;

        Vector3 direction = new Vector3(input.x, 0, input.y);

        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            Vector3 moveDir = rotation * Vector3.forward;

            float currentSpeed = _inputService.IsSprintPressed ? sprintSpeed : moveSpeed;

            Vector3 movement = moveDir.normalized * currentSpeed;
            movement.y = -9.81f;

            _characterController.Move(movement * Time.deltaTime);
        }
    }
}