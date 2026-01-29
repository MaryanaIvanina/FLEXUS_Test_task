using UnityEngine;
using Zenject;
using System.Linq;
using Content.UI;
using System.Collections.Generic;
using Content.Car.Visuals;

public class GameInteractionManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private SpeedEffect speedEffect;

    [Header("Spawn Settings")]
    [SerializeField] private List<Transform> carSpawnPoints;

    [Header("Settings")]
    [SerializeField] private float interactRadius = 3.0f;
    [SerializeField] private LayerMask interactableLayer;

    private IInputService _inputService;
    private GameFactory _gameFactory;
    private Transform _playerSpawnPoint;

    public PlayerController Player { get; private set; }
    private IControllable _currentEntity;
    private bool _isDriving = false;

    [Inject]
    public void Construct(IInputService inputService,
        GameFactory gameFactory,
        [Inject(Id = "PlayerSpawn")] Transform playerSpawn)
    {
        _inputService = inputService;
        _gameFactory = gameFactory;
        _playerSpawnPoint = playerSpawn;
    }

    private void Start()
    {
        Player = _gameFactory.CreatePlayer(_playerSpawnPoint);

        for (int i = 0; i < carSpawnPoints.Count; i++)
        {
            int carIndex = i;
            _gameFactory.CreateCar(carSpawnPoints[i], carIndex);
        }
        _inputService.OnInteract += HandleInteraction;

        _currentEntity = Player;
        SwitchControlTo(Player);

        if (speedometer != null) speedometer.Hide();

        if (speedEffect != null) speedEffect.ClearTarget();
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
        Collider[] hits = Physics.OverlapSphere(Player.transform.position, interactRadius, interactableLayer);

        IControllable nearestVehicle = hits
            .Select(h => h.GetComponentInParent<IControllable>())
            .Where(c => c != null && c != (IControllable)Player)
            .OrderBy(c => Vector3.Distance(Player.transform.position, c.transform.position))
            .FirstOrDefault();

        if (nearestVehicle != null) EnterVehicle(nearestVehicle);
    }

    private void EnterVehicle(IControllable vehicle)
    {
        _isDriving = true;

        Player.gameObject.SetActive(false);

        SwitchControlTo(vehicle);

        var rb = vehicle.GameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            if (speedometer != null) speedometer.Show(rb);

            if (speedEffect != null) speedEffect.SetTargetCar(rb);
        }
    }

    private void ExitVehicle()
    {
        Transform vehicleTransform = _currentEntity.transform;

        Vector3 exitPosition;
        Quaternion exitRotation;

        exitPosition = vehicleTransform.position - vehicleTransform.right * 2f;
        exitRotation = Quaternion.LookRotation(vehicleTransform.forward);

        _isDriving = false;

        Player.transform.SetPositionAndRotation(exitPosition, exitRotation);
        Player.gameObject.SetActive(true);

        SwitchControlTo(Player);

        if (speedometer != null) speedometer.Hide();

        if (speedEffect != null) speedEffect.ClearTarget();
    }

    private void SwitchControlTo(IControllable entity)
    {
        if (_currentEntity != null) _currentEntity.SetControlActive(false);

        _currentEntity = entity;
        _currentEntity.SetControlActive(true);

        cameraFollow.SetTarget(_currentEntity.transform);
    }
}