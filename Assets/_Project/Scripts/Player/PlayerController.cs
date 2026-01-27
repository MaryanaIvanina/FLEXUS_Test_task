using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour, IControllable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController _characterController;
    private IInputService _inputService;
    private Transform _cameraTransform;
    private Animator _animator;

    public GameObject GameObject => gameObject;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        Vector2 input = _inputService.MoveInput;

        Vector3 direction = new(input.x, 0, input.y);

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
        else
        {
            Vector3 idleMovement = new(0, -9.81f, 0);
            _characterController.Move(idleMovement * Time.deltaTime);
        }
    }

    private void UpdateAnimator()
    {
        Vector3 horizontalVelocity = new(_characterController.velocity.x, 0, _characterController.velocity.z);
        float speed = horizontalVelocity.magnitude;

        _animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    public void SetControlActive(bool isActive)
    {
        this.enabled = isActive;

        _characterController.enabled = isActive;

        if (!isActive) _animator.SetFloat("Speed", 0);
    }
}