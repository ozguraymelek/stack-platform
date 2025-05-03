using Source.Core.Utilities.External;
using Source.Data.Entities.Level;
using Source.Data.Platform;
using Source.Gameplay.Platform.Services;
using Source.Infrastructure.Signals;
using Source.Systems.Cut;
using Source.Systems.Finish;
using TMPro;
using UnityEngine;
using Zenject;

namespace Source.Systems.GameFlow
{
    public class LevelManager : MonoBehaviour
    {
        private PlatformSpawner _platformSpawner;
        private FinishSpawner _finishSpawner;
        private PlatformTracker _platformTracker;
        private SignalBus _signalBus;
        private LevelDatabase _levelDatabase;
        private CutLogic _cutLogic;
        
        public LevelEntity CurrentLevel;
        public int CurrentLevelIndex;
        
        [Inject]
        public void Construct(SignalBus signalBus, PlatformSpawner platformSpawner, FinishSpawner finishSpawner, 
            PlatformTracker platformTracker, LevelDatabase levelDatabase, CutLogic cutLogic)
        {
            _signalBus = signalBus;
            _platformSpawner = platformSpawner;
            _finishSpawner = finishSpawner;
            _platformTracker = platformTracker;
            _levelDatabase = levelDatabase;
            _cutLogic = cutLogic;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_platformSpawner), _platformSpawner),
                (nameof(_finishSpawner), _finishSpawner),
                (nameof(_platformTracker), _platformTracker),
                (nameof(_levelDatabase), _levelDatabase),
                (nameof(_cutLogic), _cutLogic)
            );
        }

        private void Start()
        {
            LoadLevel(CurrentLevelIndex);
        }

        private void LoadLevel(int index)
        {
            if (index < 0 || index >= _levelDatabase.levels.Count)
            {
                Debug.LogError("Invalid level index!");
                return;
            }

            _cutLogic.LastHullWidth = 1.5f;
            
            _cutLogic.CurrentCutter = null;
            
            CurrentLevel = _levelDatabase.levels[index];

            if (_platformTracker.InitialPlatform != null && _platformTracker.CurrentFinishPlatform != null)
            {
                CurrentLevel.AlignFirstPlatform
                    (_platformTracker.CurrentFinishPlatform, _platformTracker.InitialPlatform);
            }
            
            _finishSpawner.SpawnFinishPlatform();
            
            _platformSpawner.SpawnedPlatforms.RemoveRange(1, _platformSpawner.SpawnedPlatforms.Count - 1);
        }
        
        public void NextLevel(TMP_Text currentLevelText)
        {
            CurrentLevelIndex++;
            if (CurrentLevelIndex >= _levelDatabase.levels.Count)
            {
                CurrentLevelIndex = 0;
                Debug.LogWarning($"All levels completed, returning to the {_levelDatabase.levels[0].name}");
            }
            
            currentLevelText.text = $"Current Level: {CurrentLevelIndex + 1}";
            
            _signalBus.Fire<LevelStartedSignal>();
            LoadLevel(CurrentLevelIndex);
        }

        private void Update()
        {
            CheckCurrentLevelStatus();
        }
        
        private void CheckCurrentLevelStatus()
        {
            CurrentLevel.IsReachedPlatformLimit = _platformSpawner.SpawnedPlatforms.Count >= CurrentLevel.PlatformCountLimit;
        }
    }
}