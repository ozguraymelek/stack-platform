using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using _Project.Helper.Utils;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Game_Flow;
using _Project.Layers.Game_Logic.Game_Flow.Level_Finish;
using _Project.Layers.Game_Logic.Signals;
using _Project.Layers.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Platform
{
    public class PlatformSpawner : MonoBehaviour
    {
        public List<Platform> SpawnedPlatforms;
        
        // [Inject] private DiContainer _container;
        private SignalBus _signalBus;
        
        private ObjectPooling _platformPool;
        private PlatformTracker _platformTracker;
        private LevelManager _levelManager;
        
        [Inject]
        public void Construct(SignalBus signalBus, ObjectPooling platformPool, PlatformTracker platformTracker, LevelManager levelManager)
        {
            _signalBus = signalBus;
            _platformPool = platformPool;
            _platformTracker = platformTracker;
            _levelManager = levelManager;
        }

        private void Awake()
        {
            _platformTracker.InitialPlatform = (IInteractable<Platform>)SpawnedPlatforms[0];
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerInteractedWithPlatformSignal>(OnPlayerEnteredPlatform);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerInteractedWithPlatformSignal>(OnPlayerEnteredPlatform);
        }
        
        private void OnPlayerEnteredPlatform(PlayerInteractedWithPlatformSignal signal)
        {
            if (_levelManager.CurrentLevel.IsReachedPlatformLimit) return;
            Debug.Log("PlayerInteractedWithPlatformSignal received >> Spawning next platform!");
            
            _platformTracker.SetCurrent(signal.InteractedPlatform);
            Debug.Log($"current platform: {_platformTracker.CurrentPlatform}");

            var newPlatform = _platformPool.GetFromPool().GetComponent<Platform>();
            SpawnedPlatforms.Add(newPlatform);
            
            var isSpawnedRight = SpawnedPlatforms[^1].IsSpawnedRight;
            var spawnPos = newPlatform.transform.SetWithZRandX(SpawnedPlatforms[0].transform,
                ref isSpawnedRight,
                (SpawnedPlatforms.Count - 1) * SpawnedPlatforms[^1].transform.localScale.z, 4);
            SpawnedPlatforms[^1].IsSpawnedRight = isSpawnedRight;
            newPlatform.transform.position = spawnPos;

            // NextPlatform olarak ata
            if (newPlatform.TryGetComponent<IInteractable<Platform>>(out var interactable))
            {
                _platformTracker.SetNext(interactable);
            }
        }
    }
}