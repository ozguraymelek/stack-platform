using Source.Data.Platform;
using Source.Gameplay.Platform;
using Source.Gameplay.Platform.Services;
using Source.Infrastructure.Pools;
using Source.Infrastructure.Signals;
using Source.Systems.Finish;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class PlatformInstaller : MonoInstaller
    {
        [SerializeField] private Platform platformPrefab;
        [SerializeField] private GameObject finishPlatformPrefab;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private int initialPoolSize = 5;

        public override void InstallBindings()
        {
            Container.BindInstance(platformPrefab).WithId("Platform");
            Container.BindInstance(poolRoot).WithId("Root");
            Container.BindInstance(initialPoolSize).WithId("InitialPoolSize");

            Container.BindInterfacesAndSelfTo<ObjectPooling>().AsSingle().NonLazy();
            Container.BindFactory<FinishInteraction, FinishInteraction.Factory>()
                .FromComponentInNewPrefab(finishPlatformPrefab).AsSingle();

            Container.Bind<PlatformSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FinishSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlatformTracker>().AsSingle().NonLazy();
            Container.DeclareSignal<PlatformStopRequestedSignal>();
        }
    }
}