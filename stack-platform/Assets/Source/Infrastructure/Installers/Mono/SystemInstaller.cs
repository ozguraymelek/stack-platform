using Source.Core.Utilities.Internal;
using Source.Gameplay.Platform.Services;
using Source.Gameplay.Platform.Wrappers;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class SystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PureCoroutine>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<IAlignment>().To<PerfectAlignment>().AsSingle();
            SignalBusInstaller.Install(Container);
        }
    }
}