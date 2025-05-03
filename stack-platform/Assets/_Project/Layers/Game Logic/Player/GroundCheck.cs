using _Project.Helper.Utils;
using _Project.Helper.ZEnject;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    public interface IGroundCheckWrapper
    {
        bool CanCheck { get; set; }
        LayerMask GroundMask { get; }
    }
    public class GroundCheck : MonoBehaviour, IGroundCheckWrapper
    {
        private SignalBus _signalBus;
        
        [SerializeField] private Transform feet;
        [SerializeField] private float castMaxDist = 0.05f;
        [SerializeField] private bool grounded = true;
        
        [SerializeField] private LayerMask groundMask;
        public LayerMask GroundMask => groundMask;
        
        [SerializeField] private bool canCheck = false;
        public bool CanCheck
        {
            get => canCheck;
            set => canCheck = value;
        }

        private bool _wasGrounded = true;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus)
            );
        }
        
        private void Update()
        {
            if (CanCheck) Check();
        }

        private void Check()
        {
            var offset = new Vector3(0, 0.1f, 0);
            var origin = (feet != null ? feet.position : transform.position) + offset;
            var maxDist = castMaxDist + offset.y;
            
            grounded = Physics.Raycast(origin, Vector3.down, maxDist, groundMask);

            if (_wasGrounded && !grounded)
            {
                _wasGrounded = false;
                OnFallOff();
            }
            else if (grounded)
            {
                _wasGrounded = true;
            }
        }

        private void OnFallOff()
        {
            CanCheck = false;
            _signalBus.Fire<GameFailedSignal>();
        }
    }
}