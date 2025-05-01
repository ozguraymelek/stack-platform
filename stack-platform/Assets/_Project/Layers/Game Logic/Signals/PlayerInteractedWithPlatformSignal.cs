using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Signals
{
    public class PlayerInteractedWithPlatformSignal
    {
        public IInteractable InteractedPlatform { get; }
        public IPlatformData InteractedPlatformData { get; }

        public PlayerInteractedWithPlatformSignal(IInteractable platform, IPlatformData data)
        {
            InteractedPlatform = platform;
            InteractedPlatformData = data;
        }
    }
}
