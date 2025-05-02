using _Project.Helper.ZEnject;
using _Project.Layers.Data.Entities;
using _Project.Layers.Data.Interfaces.Player;
using _Project.Layers.Game_Logic.Game_Flow;
using _Project.Layers.Game_Logic.Game_Flow.Level_Finish;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Player;
using _Project.Layers.Game_Logic.Player.Services;
using _Project.Layers.Game_Logic.Signals;
using _Project.Layers.Infrastructure.Pools;
using _Project.Layers.Presentation;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private float defaultPlayerMovementSpeed;
        [SerializeField] private LevelDatabase levelDatabase;
        
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject finishPlatformPrefab;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private int initialPoolSize = 5;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.Bind<PureCoroutine>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<IInputProvider>().To<InputReceiver>().FromComponentInHierarchy().AsSingle();
            
            //entity
            Container.Bind<PlayerEntity>().AsSingle().WithArguments(defaultPlayerMovementSpeed);
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerApi>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerInteraction>().FromComponentInHierarchy().AsSingle().NonLazy();

            //movement
            Container.Bind<EndlessMovement>().AsSingle();
            Container.Bind<SwerveMovement>().AsSingle();
            
            //provider
            Container.BindInterfacesAndSelfTo<MovementProvider>().AsSingle().NonLazy();
            
            Container.Bind<PlatformTracker>().AsSingle().NonLazy();
            Container.BindInstance(platformPrefab).WithId("Platform");
            Container.BindInstance(poolRoot).WithId("Root");
            Container.BindInstance(initialPoolSize).WithId("InitialPoolSize");

            Container.BindInterfacesAndSelfTo<ObjectPooling>()
                .AsSingle()
                .NonLazy();
            
            
            // Container.Bind<FinishInteraction>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindFactory<FinishInteraction, FinishInteraction.Factory>()
                .FromComponentInNewPrefab(finishPlatformPrefab)
                .AsSingle();
            
            Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LevelDatabase>().FromInstance(levelDatabase).AsSingle();
            Container.Bind<PlatformSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FinishSpawner>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<GameManager>().AsSingle();
            
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<LevelStartedSignal>();
            Container.DeclareSignal<LevelFinishedSignal>();
            Container.DeclareSignal<PlayerInteractedWithPlatformSignal>();
            Container.DeclareSignal<PlayerInteractedWithFinishSignal>();
            Container.DeclareSignal<PlatformStopRequestedSignal>();
        }
    }
}
