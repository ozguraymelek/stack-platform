using Source.Core.Utilities.External;
using Source.Data.Platform;
using Source.Gameplay.Player;
using Source.Gameplay.Player.Services;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Systems.Finish
{
    public class FinishInteraction : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<FinishInteraction> { }
        
        private SignalBus _signalBus;
        private Player _player;
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
            if (!other.transform.TryGetComponent(out Source.Gameplay.Player.Player playerApi)) return;
            _signalBus.Fire(new InputToggleSignal(false));
            _signalBus.Fire<LevelFinishedSignal>();
            _platformTracker.CurrentFinishPlatform = _finish;
            _groundCheckWrapper.CanCheck = false;
        }
    }
}