using System;
using System.Collections;
using _Project.Helper.Utils;
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
        private IGroundCheckWrapper _groundCheckWrapper;

        private void Awake()
        {
            _finish = GetComponent<Finish>();
        }

        [Inject]
        public void Construct(SignalBus signalBus, PlatformTracker platformTracker,
            IGroundCheckWrapper groundCheckWrapper)
        {
            _signalBus = signalBus;
            _platformTracker = platformTracker;
            _groundCheckWrapper = groundCheckWrapper;
            
            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_platformTracker), _platformTracker),
                (nameof(_groundCheckWrapper), _groundCheckWrapper)
            );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out PlayerApi playerApi)) return;
            _signalBus.Fire<LevelFinishedSignal>();
            _platformTracker.CurrentFinishPlatform = _finish;
            _groundCheckWrapper.CanCheck = false;
        }
    }
}