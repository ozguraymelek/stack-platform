using Source.Gameplay.Platform.Wrappers;
using Source.Systems.Finish;

namespace Source.Data.Platform
{
    public class PlatformTracker
    {
        public IInteractable<Gameplay.Platform.Platform> InitialPlatform { get;  set; }
        public IInteractable<Gameplay.Platform.Platform>  CurrentPlatform { get; private set; }
        public IInteractable<Gameplay.Platform.Platform>  NextPlatform { get; private set; }
        
        public IInteractable<Finish> CurrentFinishPlatform { get; set; }
        
        public void SetInitial(IInteractable<Gameplay.Platform.Platform> platform) => InitialPlatform = platform;
        
        public void SetCurrent(IInteractable<Gameplay.Platform.Platform> platform) => CurrentPlatform = platform;

        public void SetNext(IInteractable<Gameplay.Platform.Platform> platform) => NextPlatform = platform;

        public void ClearNext() => NextPlatform = null;
    }
}