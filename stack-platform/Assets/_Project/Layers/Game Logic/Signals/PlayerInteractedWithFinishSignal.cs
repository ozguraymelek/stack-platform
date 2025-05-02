using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Game_Flow.Level_Finish;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Signals
{
    public class PlayerInteractedWithFinishSignal : MonoBehaviour
    {
        public IInteractable<Finish> InteractedPlatform { get; }
        public IPlatformData InteractedPlatformData { get; }

        public PlayerInteractedWithFinishSignal(IInteractable<Finish> platform, IPlatformData data)
        {
            InteractedPlatform = platform;
            InteractedPlatformData = data;
        }
    }
}
