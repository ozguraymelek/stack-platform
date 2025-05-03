using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using _Project.Helper.Utils;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Cut;
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
        [SerializeField] private PlatformMaterialData platformMaterialData;
        
        private SignalBus _signalBus;
        
        private IObjectPool _platformPool;
        private PlatformTracker _platformTracker;
        private LevelManager _levelManager;
        private CutLogic _cutLogic;
        private CuttedObjectConfig _cuttedObjectConfig;
        
        [Inject]
        public void Construct(SignalBus signalBus, IObjectPool platformPool, 
            PlatformTracker platformTracker, LevelManager levelManager, 
            CutLogic cutLogic, CuttedObjectConfig cuttedObjectConfig )
        {
            _signalBus = signalBus;
            _platformPool = platformPool;
            _platformTracker = platformTracker;
            _levelManager = levelManager;
            _cutLogic = cutLogic;
            _cuttedObjectConfig = cuttedObjectConfig;
        }

        private void Awake()
        {
            _platformTracker.InitialPlatform = (IInteractable<Platform>)SpawnedPlatforms[0];
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerInteractedWithPlatformSignal>(OnPlayerEnteredPlatform);
            _cuttedObjectConfig.LeftHull.Width = 1.5f;
            _cuttedObjectConfig.RightHull.Width = 1.5f;
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerInteractedWithPlatformSignal>(OnPlayerEnteredPlatform);
        }
        
        private void OnPlayerEnteredPlatform(PlayerInteractedWithPlatformSignal signal)
        {
            if (_levelManager.CurrentLevel.IsReachedPlatformLimit == false &&
                 _platformTracker.InitialPlatform != null)
            {
                _platformTracker.SetCurrent(signal.InteractedPlatform);

                var spawnedPlatform = SpawnProcess();
                
                CutProcessSettings(spawnedPlatform);
                VisualSettings(spawnedPlatform);
                
                if (spawnedPlatform.TryGetComponent<IInteractable<Platform>>(out var interactable))
                {
                    _platformTracker.SetNext(interactable);
                }
            }
        }

        private Platform SpawnProcess()
        {
            var spawnedPlatform = _platformPool.Get().GetReference();
            SpawnedPlatforms.Add(spawnedPlatform);
            return spawnedPlatform;
        }

        private void CutProcessSettings(Platform platform)
        {
            platform.PlatformPool = _platformPool;

            CutterWidth(platform);
            
            var isSpawnedRight = SpawnedPlatforms[^1].IsSpawnedRight;
            
            var spawnPos = platform.transform.SetWithZRandX(SpawnedPlatforms[0].transform,
                ref isSpawnedRight,
                (SpawnedPlatforms.Count - 1) * SpawnedPlatforms[^1].transform.localScale.z, 4);
            
            SpawnedPlatforms[^1].IsSpawnedRight = isSpawnedRight;
            
            platform.transform.position = spawnPos;
        }

        private void CutterWidth(Platform platform)
        {
            if (_cutLogic.CurrentCutter != null && _cutLogic.CurrentCutter.IsActiveHullOnLeft == true)
            {
                platform.transform.localScale = new Vector3(_cuttedObjectConfig.LeftHull.Width, 0.1f, 2f);
            }
            else if (_cutLogic.CurrentCutter != null && _cutLogic.CurrentCutter.IsActiveHullOnRight == true)
            {
                platform.transform.localScale = new Vector3(_cuttedObjectConfig.RightHull.Width, 0.1f, 2f);
            }
            else
            {
                platform.transform.localScale = new Vector3(_cutLogic.LastHullWidth, 0.1f, 2f);
            }
        }

        private void VisualSettings(Platform platform)
        {
            platform.GetRenderer().material = SRandom.Material(platformMaterialData.materials);
        }
    }
}