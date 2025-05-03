using Source.Data.Cut;
using Source.Infrastructure.Signals;
using Source.Systems.Cut;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers.Mono
{
    public class CutInstaller : MonoInstaller
    {
        [SerializeField] private Cutter cutterPrefab;
        
        public override void InstallBindings()
        {
            Container.BindFactory<Cutter, Cutter.Factory>()
                .FromComponentInNewPrefab(cutterPrefab);

            Container.Bind<CutLogic>().FromComponentInHierarchy().AsSingle();
            Container.DeclareSignal<CutRequestSignal>();
        }
    }
}