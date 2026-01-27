using UnityEngine;
using Zenject;

public class CarController : MonoBehaviour, IControllable
{
    [Header("Settings")]
    [SerializeField] private float motorForce = 10000f; 
    [SerializeField] private float brakeForce = 50000f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftCollider;
    [SerializeField] private WheelCollider frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider;
    [SerializeField] private WheelCollider rearRightCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftMesh;
    [SerializeField] private Transform frontRightMesh;
    [SerializeField] private Transform rearLeftMesh;
    [SerializeField] private Transform rearRightMesh;

    private IInputService _inputService;
    private Vector2 _currentInput; 
    private bool _isBraking;

    public GameObject GameObject => gameObject;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Update()
    {
        _currentInput = _inputService.MoveInput;
        _isBraking = _inputService.IsBraking;
        ApplyMotor();
        ApplySteering();
        UpdateWheels();
    }

    private void ApplyMotor()
    {
        float currentForce = _isBraking ? 0 : _currentInput.y * motorForce;

        frontLeftCollider.motorTorque = currentForce;
        frontRightCollider.motorTorque = currentForce;

        float currentBrake = _isBraking ? brakeForce : 0f;
        ApplyBrakeToAllWheels(currentBrake);
    }

    private void ApplyBrakeToAllWheels(float force)
    { 
        frontLeftCollider.brakeTorque = force;
        frontRightCollider.brakeTorque = force;
        rearLeftCollider.brakeTorque = force;
        rearRightCollider.brakeTorque = force;
    }

    private void ApplySteering()
    {
        float steering = _currentInput.x * maxSteerAngle;

        frontLeftCollider.steerAngle = steering;
        frontRightCollider.steerAngle = steering;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftCollider, frontLeftMesh);
        UpdateSingleWheel(frontRightCollider, frontRightMesh);
        UpdateSingleWheel(rearLeftCollider, rearLeftMesh);
        UpdateSingleWheel(rearRightCollider, rearRightMesh);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);

        wheelTransform.SetPositionAndRotation(pos, rot);
    }
    public void SetControlActive(bool isActive)
    {
        this.enabled = isActive;

        if (!isActive)
        {
            frontLeftCollider.brakeTorque = brakeForce;
            frontRightCollider.brakeTorque = brakeForce;
            rearLeftCollider.brakeTorque = brakeForce;
            rearRightCollider.brakeTorque = brakeForce;

            frontLeftCollider.motorTorque = 0;
            frontRightCollider.motorTorque = 0;
        }
    }
}