using System.Collections.Generic;
using System.Runtime.InteropServices;
using _Project.Helper.Utils;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Signals;
using _Project.Layers.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Platform
{
    public class PlatformSpawner : MonoBehaviour
    {
        public List<Platform> SpawnedPlatforms;
        
        [Inject] private DiContainer _container;
        private SignalBus _signalBus;
        
        private ObjectPooling _platformPool;
        private PlatformTracker _platformTracker;
        
        [Inject]
        public void Construct(SignalBus signalBus, ObjectPooling platformPool, PlatformTracker platformTracker)
        {
            _signalBus = signalBus;
            _platformPool = platformPool;
            _platformTracker = platformTracker;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerInteractedWithPlatformSignal>(OnPlayerEnteredPlatform);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerInteractedWithPlatformSignal>(OnPlayerEnteredPlatform);
        }
        
        public void OnPlayerEnteredPlatform(PlayerInteractedWithPlatformSignal signal)
        {
            Debug.Log("PlayerInteractedWithPlatformSignal received >> Spawning next platform!");
            // Önce CurrentPlatform'u güncelle
            _platformTracker.SetCurrent(signal.InteractedPlatform);
            Debug.Log($"current platform: {_platformTracker.CurrentPlatform}");

            // Yeni platform spawnla
            var newPlatform = _platformPool.GetFromPool().GetComponent<Platform>();
            SpawnedPlatforms.Add(newPlatform);
            // Pozisyon hesapla ve ayarla
            var isSpawnedRight = SpawnedPlatforms[^1].IsSpawnedRight;
            var spawnPos = newPlatform.transform.SetWithZRandX(SpawnedPlatforms[0].transform,
                ref isSpawnedRight,
                (SpawnedPlatforms.Count - 1) * SpawnedPlatforms[^1].transform.localScale.z, 4);
            SpawnedPlatforms[^1].IsSpawnedRight = isSpawnedRight;
            newPlatform.transform.position = spawnPos;

            // NextPlatform olarak ata
            if (newPlatform.TryGetComponent<IInteractable>(out var interactable))
            {
                _platformTracker.SetNext(interactable);
            }
        }
    }
}