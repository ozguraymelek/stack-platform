using System;
using _Project.Layers.Game_Logic.Game_Flow.Level_Finish;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Platform
{
    public class PlatformTracker
    {
        public IInteractable<Platform> InitialPlatform { get;  set; }
        public IInteractable<Platform>  CurrentPlatform { get; private set; }
        public IInteractable<Platform>  NextPlatform { get; private set; }


        public IInteractable<Finish> CurrentFinishPlatform { get; set; }
        
        public void SetInitial(IInteractable<Platform> platform) => InitialPlatform = platform;
        
        public void SetCurrent(IInteractable<Platform> platform) => CurrentPlatform = platform;

        public void SetNext(IInteractable<Platform> platform) => NextPlatform = platform;

        public void ClearNext() => NextPlatform = null;
    }
}
