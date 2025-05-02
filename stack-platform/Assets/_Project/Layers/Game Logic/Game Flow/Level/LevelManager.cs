using System;
using _Project.Layers.Data.Entities;
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
        
        public LevelEntity CurrentLevel;
        public int CurrentLevelIndex;
        public int CurrentPlatformCount = 0;
        
        [Inject]
        public void Construct(PlatformSpawner platformSpawner, FinishSpawner finishSpawner, PlatformTracker platformTracker, LevelDatabase levelDatabase, SignalBus signalBus)
        {
            _platformSpawner = platformSpawner;
            _finishSpawner = finishSpawner;
            _platformTracker = platformTracker;
            _levelDatabase = levelDatabase;
            _signalBus = signalBus;
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
            
            CurrentLevel = _levelDatabase.levels[index];
            var levelData = _levelDatabase.levels[index];
            CurrentPlatformCount = 0;
            
            if (CurrentLevelIndex != 0)
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
            currentLevelText.text = $"Current Level: {CurrentLevelIndex + 1}";
            if (CurrentLevelIndex >= _levelDatabase.levels.Count)
            {
                CurrentLevelIndex = 0;
                Debug.LogError("All levels have been loaded");
            }

            _signalBus.Fire<LevelStartedSignal>();
            // FindObjectOfType<Player>().animator.SetBool("Dance", false);
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