using _Project.Layers.Data.Entities;
using _Project.Layers.Data.Interfaces.Player;
using _Project.Layers.Game_Logic.Player;
using _Project.Layers.Game_Logic.Player.Services;
using _Project.Layers.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private float defaultPlayerMovementSpeed;
        
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private int initialPoolSize = 5;
        public override void InstallBindings()
        {
            //entity
            Container.Bind<PlayerEntity>().AsSingle().WithArguments(defaultPlayerMovementSpeed);
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle().NonLazy();
            
            //movement
            Container.Bind<EndlessMovement>().AsSingle();
            Container.Bind<SwerveMovement>().AsSingle();
            
            //provider
            Container.BindInterfacesAndSelfTo<MovementProvider>().AsSingle().NonLazy();
            
            
            Container.BindInstance(platformPrefab).WithId("Platform");
            Container.BindInstance(poolRoot).WithId("Root");
            Container.BindInstance(initialPoolSize).WithId("InitialPoolSize");

            Container.BindInterfacesAndSelfTo<ObjectPooling>()
                .AsSingle()
                .NonLazy();

        }
    }
}
