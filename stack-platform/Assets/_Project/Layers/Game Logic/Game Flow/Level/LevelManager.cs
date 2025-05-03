using System;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Cut;
using _Project.Layers.Game_Logic.Game_Flow.Level_Finish;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Signals;
using _Project.Layers.Infrastructure;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Game_Flow
{
    public class LevelManager : MonoBehaviour
    {
        private PlatformSpawner _platformSpawner;
        private FinishSpawner _finishSpawner;
        private PlatformTracker _platformTracker; //platform runtime data
        private SignalBus _signalBus;
        private LevelDatabase _levelDatabase;
        private CutLogic _cutLogic;
        
        public LevelEntity CurrentLevel;
        public int CurrentLevelIndex;
        
        [Inject]
        public void Construct(PlatformSpawner platformSpawner, FinishSpawner finishSpawner, 
            PlatformTracker platformTracker, LevelDatabase levelDatabase, SignalBus signalBus, CutLogic cutLogic)
        {
            _platformSpawner = platformSpawner;
            _finishSpawner = finishSpawner;
            _platformTracker = platformTracker;
            _levelDatabase = levelDatabase;
            _signalBus = signalBus;
            _cutLogic = cutLogic;
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
            
            CurrentLevel.IsReachedTarget = false;
            
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