using Source.Core.Utilities.External;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Gameplay.Player.Services
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
        [SerializeField] private Transform rightFoot;
        [SerializeField] private Transform leftFoot;
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
            if (CanCheck)
            {
                Check();
            }
        }

        private void Check()
        {
            var offset = new Vector3(0, 0.1f, 0);
            var origin = (feet != null ? feet.position : transform.position) + offset;
            var maxDist = castMaxDist + offset.y;
            
            grounded = Physics.CheckSphere(origin,  maxDist, groundMask);

            if (_wasGrounded && !grounded)
            {
                _wasGrounded = false;
                OnFallOff();
            }
            else if (grounded)
                _wasGrounded = true;
        }

        private void OnFallOff()
        {
            CanCheck = false;
            _signalBus.Fire<GameFailedSignal>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(feet.position, castMaxDist + 0.1f);
        }
    }
}