using System;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Platform
{
    public class PlatformTracker
    {
        public Platform InitialPlatform { get; private set; }
        public IInteractable CurrentPlatform { get; private set; }
        public IInteractable NextPlatform { get; private set; }
        
        public void SetCurrent(IInteractable platform)
        {
            CurrentPlatform = platform;
        }

        public void SetNext(IInteractable platform)
        {
            NextPlatform = platform;
        }

        public void ClearNext()
        {
            NextPlatform = null;
        }
    }
}
