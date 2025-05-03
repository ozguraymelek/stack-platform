using Source.Infrastructure.Signals;
using Source.UI.Input;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputProvider>().To<InputReceiver>().FromComponentInHierarchy().AsSingle();
            Container.DeclareSignal<InputToggleSignal>();
        }
    }
}