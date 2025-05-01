using _Project.Layers.Game_Logic.Platform;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Game_Flow
{
    public class LevelManager : MonoBehaviour
    {
        private readonly PlatformSpawner _platformSpawner;
        private readonly PlatformTracker _platformTracker;
        private readonly SignalBus _signalBus;
        
        public LevelManager(PlatformSpawner platformSpawner, PlatformTracker platformTracker, SignalBus signalBus)
        {
            _platformSpawner = platformSpawner;
            _platformTracker = platformTracker;
            _signalBus = signalBus;
        }
        
        public void LoadLevel(int index)
        {
            // PlatformTracker’ı sıfırla
            // _platformTracker.Reset();
            //
            // // İlk platformu spawnla ve tracker’a ayarla
            // var firstPlatform = _platformSpawner.SpawnInitialPlatform();
            // _platformTracker.SetCurrentPlatform(firstPlatform);
            //
            // // Gerekirse sinyal yay (GameStartedSignal mesela)
            // _signalBus.Fire<GameStartedSignal>();
            //
            // Debug.Log("Level started");
        }
    }
}
