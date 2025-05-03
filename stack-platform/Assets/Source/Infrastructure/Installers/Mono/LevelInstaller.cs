using Source.Data.Entities.Level;
using Source.Systems.GameFlow;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelDatabase levelDatabase;

        public override void InstallBindings()
        {
            Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LevelDatabase>().FromInstance(levelDatabase).AsSingle();
        }
    }
}