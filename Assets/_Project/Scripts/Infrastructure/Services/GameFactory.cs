using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class GameFactory
{
    private readonly DiContainer _container;

    private readonly GameObject _playerPrefab;
    private readonly List<GameObject> _carPrefabs; 

    
    public GameFactory(DiContainer container,
                       [Inject(Id = "Player")] GameObject playerPrefab,
                       [Inject(Id = "CarPrefabs")] List<GameObject> carPrefabs)
    {
        _container = container;
        _playerPrefab = playerPrefab;
        _carPrefabs = carPrefabs;
    }

    public PlayerController CreatePlayer(Transform spawnPoint)
    {
        return _container.InstantiatePrefabForComponent<PlayerController>(_playerPrefab, spawnPoint.position, spawnPoint.rotation, null);
    }

    public CarController CreateCar(Transform spawnPoint, int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= _carPrefabs.Count)
        {
            Debug.LogError($"GameFactory: Немає машини з індексом {prefabIndex}. Спавню першу.");
            prefabIndex = 0;
        }

        GameObject selectedPrefab = _carPrefabs[prefabIndex];

        return _container.InstantiatePrefabForComponent<CarController>(selectedPrefab, spawnPoint.position, spawnPoint.rotation, null);
    }
}