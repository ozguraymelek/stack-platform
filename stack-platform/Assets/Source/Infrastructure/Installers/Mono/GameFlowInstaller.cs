using Source.Infrastructure.Signals;
using Source.Systems.GameFlow;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class GameFlowInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle();
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<LevelStartedSignal>();
            Container.DeclareSignal<LevelFinishedSignal>();
            Container.DeclareSignal<GameFailedSignal>();
            Container.DeclareSignal<StreakSignal>();
            Container.DeclareSignal<StreakLostSignal>();
        }
    }
}