using System.Collections;
using System.Collections.Generic;
using _Project.Layers.Game_Logic.Cut;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CutterObjectInstaller", menuName = "Installers/CutterObjectInstaller")]
public class CutterObjectInstaller : ScriptableObjectInstaller<CutterObjectInstaller>
{
    public CutterObjectConfig CutterObjectConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(CutterObjectConfig).AsSingle();
    }
}
