using UnityEngine;
using Zenject;
using System.Linq;

public class GameInteractionManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraFollow cameraFollow;

    [Header("Settings")]
    [SerializeField] private float interactRadius = 3.0f;
    [SerializeField] private LayerMask interactableLayer; 

    private IInputService _inputService;
    private IControllable _currentEntity; 
    private bool _isDriving = false;

    [Inject]
    public void Construct(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void Start()
    {
        _inputService.OnInteract += HandleInteraction;

        _currentEntity = playerController;
        SwitchControlTo(playerController);
    }

    private void OnDestroy()
    {
        if (_inputService != null) _inputService.OnInteract -= HandleInteraction;
    }

    private void HandleInteraction()
    {
        if (_isDriving) { ExitVehicle(); }
        else { TryEnterVehicle(); }
    }

    private void TryEnterVehicle()
    {
        Collider[] hits = Physics.OverlapSphere(playerController.transform.position, interactRadius, interactableLayer);

        IControllable nearestVehicle = hits
            .Select(h => h.GetComponentInParent<IControllable>())
            .Where(c => c != null && c != (IControllable)playerController) 
            .OrderBy(c => Vector3.Distance(playerController.transform.position, c.transform.position)) 
            .FirstOrDefault(); 

        if (nearestVehicle != null) EnterVehicle(nearestVehicle);
    }

    private void EnterVehicle(IControllable vehicle)
    {
        _isDriving = true;

        playerController.gameObject.SetActive(false);

        SwitchControlTo(vehicle);
    }

    private void ExitVehicle()
    {
        Transform vehicleTransform = _currentEntity.transform;

        Vector3 exitPosition;
        Quaternion exitRotation;

        exitPosition = vehicleTransform.position - vehicleTransform.right * 2f;
        exitRotation = Quaternion.LookRotation(vehicleTransform.forward);

        _isDriving = false;

        playerController.transform.SetPositionAndRotation(exitPosition, exitRotation);
        playerController.gameObject.SetActive(true);

        SwitchControlTo(playerController);
    }

    private void SwitchControlTo(IControllable entity)
    {
        if (_currentEntity != null) _currentEntity.SetControlActive(false);

        _currentEntity = entity;
        _currentEntity.SetControlActive(true);

        cameraFollow.SetTarget(_currentEntity.transform);
    }
}