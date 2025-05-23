using System.Collections.Generic;
using Source.Core.Utilities.External;
using Source.Data.Cut;
using Source.Data.Platform;
using Source.Gameplay.Platform.Wrappers;
using Source.Infrastructure.Pools;
using Source.Infrastructure.Signals;
using Source.Systems.Cut;
using Source.Systems.GameFlow;
using UnityEngine;
using Zenject;

namespace Source.Gameplay.Platform.Services
{
    public class PlatformSpawner : MonoBehaviour
    {
        private DiContainer _container;
        private SignalBus _signalBus;
        private IObjectPool _platformPool;
        private PlatformTracker _platformTracker;
        private LevelManager _levelManager;
        private CutLogic _cutLogic;
        private CuttedObjectConfig _cuttedObjectConfig;
        
        public List<Platform> SpawnedPlatforms;
        [SerializeField] private PlatformMaterialData platformMaterialData;
        
        [Inject]
        public void Construct(SignalBus signalBus, IObjectPool platformPool, 
            PlatformTracker platformTracker, LevelManager levelManager, 
            CutLogic cutLogic, CuttedObjectConfig cuttedObjectConfig, DiContainer container)
        {
            _container = container;
            _signalBus = signalBus;
            _platformPool = platformPool;
            _platformTracker = platformTracker;
            _levelManager = levelManager;
            _cutLogic = cutLogic;
            _cuttedObjectConfig = cuttedObjectConfig;

            SLog.InjectionStatus(this,
                (nameof(_container), _container),
                (nameof(_signalBus), _signalBus),
                (nameof(_platformPool), _platformPool),
                (nameof(_platformTracker), _platformTracker),
                (nameof(_levelManager), _levelManager),
                (nameof(_cutLogic), _cutLogic),
                (nameof(_cuttedObjectConfig), _cuttedObjectConfig)
            );
        }

        private void Awake()
        {
            _platformTracker.SetInitial(SpawnedPlatforms[0]);
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

                _signalBus.Fire(new InputToggleSignal(true));
            }
        }

        private Platform SpawnProcess()
        {
            var handle = _platformPool.Get();
            var spawnedPlatform = handle.GetReference();
            _container.InjectGameObject(spawnedPlatform.gameObject);
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