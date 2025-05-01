using System;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Infrastructure;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Game_Flow
{
    public class LevelManager : MonoBehaviour
    {
        private PlatformSpawner _platformSpawner;
        private PlatformTracker _platformTracker; //platform runtime data
        private SignalBus _signalBus;
        private LevelDatabase _levelDatabase;
        
        public LevelEntity CurrentLevel;
        public int CurrentLevelIndex;
        public int CurrentPlatformCount = 0;
        
        [Inject]
        public void Construct(PlatformSpawner platformSpawner, PlatformTracker platformTracker, LevelDatabase levelDatabase, SignalBus signalBus)
        {
            _platformSpawner = platformSpawner;
            _platformTracker = platformTracker;
            _levelDatabase = levelDatabase;
            _signalBus = signalBus;
        }

        private void Start()
        {
            LoadLevel(CurrentLevelIndex);
        }

        public void LoadLevel(int index)
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
                // CurrentLevel.AlignFirstPlatform
                //     (PlatformRuntimeData.CurrentFinishPlatform, PlatformRuntimeData.FirstPlatform);
            }
            
            // FinishPlatformSpawner.Instance.SpawnFinishPlatform();
            CurrentLevel.IsReachedTarget = false;
            _platformSpawner.SpawnedPlatforms.RemoveRange(1, _platformSpawner.SpawnedPlatforms.Count - 1);
        }
    }
}
