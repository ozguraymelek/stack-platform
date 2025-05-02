using System;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Signals;
using _Project.Layers.Presentation;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private SignalBus _signalBus;
        private IInputProvider _inputProvider;

        private PlatformTracker _platformTracker;
        
        [Inject]
        public void Construct(SignalBus signalBus, IInputProvider inputProvider)
        {
            _signalBus = signalBus;
            _inputProvider = inputProvider;
        }

        private void Update()
        {
            if (_inputProvider.ClickedLeftMouse())
            {
                _signalBus.Fire<PlatformStopRequestedSignal>();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent<IInteractable<Platform.Platform>>(out var currentInteractable)) return;
            if (!other.transform.TryGetComponent<IPlatformData>(out var currentPlatformData)) return;
            Debug.Log($"Player entered {currentPlatformData.GetTransform().name}");
            _signalBus.Fire(new PlayerInteractedWithPlatformSignal(currentInteractable, currentPlatformData));
        }
    }
}