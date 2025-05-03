using Source.Core.Utilities.External;
using Source.Data.Entities;
using Source.Gameplay.Platform.Wrappers;
using Source.Infrastructure.Signals;
using Source.UI.Input;
using UnityEngine;
using Zenject;

namespace Source.Gameplay.Player.Services
{
    public class PlayerInteraction : MonoBehaviour
    {
        private SignalBus _signalBus;
        private IInputProvider _inputProvider;
        private IGroundCheckWrapper _groundCheckWrapper;
        
        [Inject]
        public void Construct(SignalBus signalBus, IInputProvider inputProvider,
            IGroundCheckWrapper groundCheckWrapper)
        {
            _signalBus = signalBus;
            _inputProvider = inputProvider;
            _groundCheckWrapper = groundCheckWrapper;
            
            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_inputProvider), _inputProvider),
                (nameof(_groundCheckWrapper), _groundCheckWrapper)
            );
        }

        private void Update()
        {
            if (_inputProvider.ClickedLeftMouse())
            {
                _signalBus.Fire(new PlatformStopRequestedSignal());
                _signalBus.Fire(new CutRequestSignal());
                _signalBus.Fire(new InputToggleSignal(false));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_groundCheckWrapper.GroundMask & (1 << other.gameObject.layer)) != 0) _groundCheckWrapper.CanCheck = true;
            if (!other.transform.TryGetComponent<IInteractable<Platform.Platform>>(out var currentInteractable)) return;
            if (!other.transform.TryGetComponent<IPlatformData>(out var currentPlatformData)) return;
            
            _signalBus.Fire(new PlayerInteractedWithPlatformSignal(currentInteractable, currentPlatformData));
        }
    }
}