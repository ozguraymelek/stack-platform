using System.ComponentModel;
using Source.Data.Cut;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Installers.SObj
{
    
    [CreateAssetMenu(menuName = "Installers/CutConfigsInstaller")]
    public class CutConfigsInstaller : ScriptableObjectInstaller<CutConfigsInstaller>
    {
        public CutLogicConfig cutLogicConfig;
        public CuttedObjectConfig cuttedObjectConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(cutLogicConfig).AsSingle();
            Container.BindInstance(cuttedObjectConfig).AsSingle();
        }
    }
}