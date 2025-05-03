using Source.Data.Entities;
using Source.Gameplay.Player;
using Source.Gameplay.Player.Services;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private float defaultSpeed;

        public override void InstallBindings()
        {
            Container.Bind<PlayerEntity>().AsSingle().WithArguments(defaultSpeed);
            Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerInteraction>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GroundCheck>().FromComponentInHierarchy().AsSingle();

            Container.DeclareSignal<PlayerInteractedWithPlatformSignal>();
            Container.DeclareSignal<MovementToggleSignal>();
            Container.DeclareSignal<PhysicToggleSignal>();
        }
    }
}