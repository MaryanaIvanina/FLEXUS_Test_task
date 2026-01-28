using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class GameplaySceneInstaller : MonoInstaller
{
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<GameObject> carPrefabs; // <--- ÒÓÒ ÑÏÈÑÎÊ

    [Header("Spawn Points")]
    [SerializeField] private Transform playerSpawnPoint;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().WithId("Player").FromInstance(playerPrefab);

        Container.Bind<List<GameObject>>().WithId("CarPrefabs").FromInstance(carPrefabs);

        Container.Bind<GameFactory>().AsSingle();

        Container.Bind<Transform>().WithId("PlayerSpawn").FromInstance(playerSpawnPoint);

        Container.Bind<GameInteractionManager>().FromComponentInHierarchy().AsSingle();
    }
}