using Source.Data.Entities;
using Source.Gameplay.Platform;
using Source.Gameplay.Platform.Wrappers;

namespace Source.Infrastructure.Signals
{
    public class PlayerInteractedWithPlatformSignal
    {
        public IInteractable<Platform> InteractedPlatform { get; }
        public IPlatformData InteractedPlatformData { get; }

        public PlayerInteractedWithPlatformSignal(IInteractable<Platform> platform, IPlatformData data)
        {
            InteractedPlatform = platform;
            InteractedPlatformData = data;
        }
    }
}