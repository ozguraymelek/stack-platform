using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Presentation
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
            Debug.LogError("InputReceiver: OnToggleInput called");
            InputEnabled = signal.Enable;
        }
        
        public bool ClickedLeftMouse()
        {
            return InputEnabled && Input.GetMouseButtonDown(0);
        }
    }
}