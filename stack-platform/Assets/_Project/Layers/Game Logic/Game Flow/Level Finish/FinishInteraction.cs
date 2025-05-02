using System;
using System.Collections;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Player;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Game_Flow.Level_Finish
{
    public class FinishInteraction : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<FinishInteraction> { }
        
        private SignalBus _signalBus;
        private PlayerApi _playerApi;
        private PlatformTracker _platformTracker;
        private Finish _finish;

        private void Awake()
        {
            _finish = GetComponent<Finish>();
        }

        [Inject]
        public void Construct(SignalBus signalBus, PlatformTracker platformTracker)
        {
            _signalBus = signalBus;
            _platformTracker = platformTracker;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerInteractedWithFinishSignal>(OnPlayerReachedFinish);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerInteractedWithFinishSignal>(OnPlayerReachedFinish);
        }

        private void OnPlayerReachedFinish(PlayerInteractedWithFinishSignal signal)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out PlayerApi playerApi)) return;
            _signalBus.Fire<LevelFinishedSignal>();
            _platformTracker.CurrentFinishPlatform = _finish;
            
            Debug.Log($"Player entered {_platformTracker.CurrentFinishPlatform.GetTransform().name}");
        }
    }
}
