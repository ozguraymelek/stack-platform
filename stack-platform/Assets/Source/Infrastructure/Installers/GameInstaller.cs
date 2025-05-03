using Source.Core.Utilities.Internal;
using Source.Data.Cut;
using Source.Data.Entities;
using Source.Data.Entities.Level;
using Source.Data.Platform;
using Source.Gameplay.Platform;
using Source.Gameplay.Platform.Services;
using Source.Gameplay.Platform.Wrappers;
using Source.Gameplay.Player;
using Source.Gameplay.Player.Services;
using Source.Infrastructure.Pools;
using Source.Infrastructure.Signals;
using Source.Systems.Cut;
using Source.Systems.Finish;
using Source.Systems.GameFlow;
using Source.UI.Input;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GroundCheck groundCheck;
        
        [SerializeField] private float defaultPlayerMovementSpeed;
        [SerializeField] private LevelDatabase levelDatabase;
        
        [SerializeField] private Platform platformPrefab;
        [SerializeField] private GameObject finishPlatformPrefab;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private int initialPoolSize = 5;

        public Cutter CutterPrefab;
        
        public CutLogicConfig CutLogicConfig;
        public CuttedObjectConfig CuttedObjectConfig;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.Bind<PureCoroutine>().FromNewComponentOnNewGameObject().AsSingle();

            Container.Bind<IInputProvider>().To<InputReceiver>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GroundCheck>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            //entity
            Container.Bind<PlayerEntity>().AsSingle().WithArguments(defaultPlayerMovementSpeed);
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerInteraction>().FromComponentInHierarchy().AsSingle().NonLazy();

            Container.Bind<PlatformTracker>().AsSingle().NonLazy();
            
            Container.BindInstance(platformPrefab).WithId("Platform");
            Container.BindInstance(poolRoot).WithId("Root");
            Container.BindInstance(initialPoolSize).WithId("InitialPoolSize");
            
            Container.BindInstance(CutLogicConfig).AsSingle().NonLazy();
            Container.BindInstance(CuttedObjectConfig).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<ObjectPooling>()
                .AsSingle()
                .NonLazy();
            
            Container.BindFactory<FinishInteraction, FinishInteraction.Factory>()
                .FromComponentInNewPrefab(finishPlatformPrefab)
                .AsSingle();
            
            Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LevelDatabase>().FromInstance(levelDatabase).AsSingle();
            Container.Bind<PlatformSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FinishSpawner>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<GameManager>().AsSingle();
            
            Container.BindFactory<Cutter, Cutter.Factory>()
                .FromComponentInNewPrefab(CutterPrefab);
            
            Container.Bind<CutLogic>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<IAlignment>().To<PerfectAlignment>().AsSingle();
            
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<LevelStartedSignal>();
            Container.DeclareSignal<LevelFinishedSignal>();
            Container.DeclareSignal<GameFailedSignal>();
            Container.DeclareSignal<PlayerInteractedWithPlatformSignal>();
            Container.DeclareSignal<InputToggleSignal>();
            Container.DeclareSignal<CutRequestSignal>();
            Container.DeclareSignal<PlatformStopRequestedSignal>();
            Container.DeclareSignal<StreakSignal>();
            Container.DeclareSignal<StreakLostSignal>();
            Container.DeclareSignal<MovementToggleSignal>();
            Container.DeclareSignal<PhysicToggleSignal>();
        }
    }
}