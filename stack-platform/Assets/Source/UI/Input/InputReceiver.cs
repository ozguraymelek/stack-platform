using Source.Core.Utilities.External;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.UI.Input
{
    public class InputReceiver : MonoBehaviour, IInputProvider
    {
        private SignalBus _signalBus;
        
        [SerializeField] private bool inputEnabled = false;
        public bool InputEnabled
        {
            get => inputEnabled;
            set => inputEnabled = value;
        }
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus)
            );
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<InputToggleSignal>(OnToggleInput);
        }
        
        private void OnDisable()
        {
            _signalBus.Unsubscribe<InputToggleSignal>(OnToggleInput);
        }
        
        private void OnToggleInput(InputToggleSignal signal)
        {
            InputEnabled = signal.Enable;
        }
        
        public bool ClickedLeftMouse()
        {
            return InputEnabled && UnityEngine.Input.GetMouseButtonDown(0);
        }
    }
}