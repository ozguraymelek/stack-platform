using Source.Core.Utilities.External;
using Source.Data.Entities;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Gameplay.Platform.Services
{
    public class PlatformMovement : MonoBehaviour
    {
        private SignalBus _signalBus;
        private IPlatformData _currentPlatformData;
        
        [SerializeField] private float moveSpeed = 2f;
        private bool _isMoving = true;
        
        public Vector3 MovementDirection = Vector3.right;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus)
            );
        }

        private void Awake()
        {
            _currentPlatformData = GetComponent<Platform>();
        }

        private void OnEnable()
        {
            _isMoving = true;
            _signalBus.Subscribe<PlatformStopRequestedSignal>(OnStopRequested);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlatformStopRequestedSignal>(OnStopRequested);
        }

        private void Update()
        {
            if (_isMoving == false) return;

            transform.position += new Vector3(
                (_currentPlatformData.IsSpawnedRight ? -MovementDirection.x : MovementDirection.x) * moveSpeed *
                Time.deltaTime, 0, 0);
        }
        
        private void OnStopRequested()
        {
            _isMoving = false;
        }
    }
}