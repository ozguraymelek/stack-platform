using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public class PlatformTracker : MonoBehaviour
    {
        public IPlatformInteractable CurrentPlatform { get; private set; }
        public IPlatformInteractable NextPlatform { get; private set; }
    }
}
