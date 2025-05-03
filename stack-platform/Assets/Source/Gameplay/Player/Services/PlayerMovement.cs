using Source.Core.Utilities.External;
using Source.Data.Entities;
using Source.Data.Platform;
using Source.Infrastructure.Signals;
using Source.Systems.Cut;
using UnityEngine;
using Zenject;

namespace Source.Gameplay.Player.Services
{
    public class PlayerMovement : MonoBehaviour
    {
        [Inject] private PlayerEntity _playerEntity;
        
        private SignalBus _signalBus;
        private CutLogic _cutLogic;
        private PlatformTracker _platformTracker;
        
        public Vector3 MovementDirection;
        
        public float MovementSpeed;
        public float RotationSpeed;
        public float Distance;

        public float Angle;
        public Vector3 Direction;
        public float targetY;
        private Quaternion _targetRotation;

        public bool IsActiveHullApproachCompleted = false;
        
        [SerializeField] private float distanceThreshold;
        [SerializeField] private float angleThreshold;
        
        [Inject]
        public void Construct(SignalBus signalBus, CutLogic cutLogic, PlatformTracker platformTracker)
        {
            _signalBus = signalBus;
            _cutLogic = cutLogic;
            _platformTracker = platformTracker;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_cutLogic), _cutLogic),
                (nameof(_platformTracker), _platformTracker)
            );
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<MovementToggleSignal>(OnToggleMovement);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<MovementToggleSignal>(OnToggleMovement);
        }

        private void Update()
        {
            if (_playerEntity.IsMovementEnable == false)
            {
                MovementDirection = Vector3.zero;
                return;
            }
            
            if (TryApproachToActiveHull()) HandleApproach();
            else if (IsActiveHullApproachCompleted == true) HandleNextPlatformAlignment();
            else MovementDirection = Vector3.forward;
            
            transform.position += MovementDirection.normalized * (Time.deltaTime * MovementSpeed);
        }
        
        private bool TryApproachToActiveHull()
        {
            if (_cutLogic.CurrentCutter == null) return false;
            return (_cutLogic.CurrentCutter.IsActiveHullOnLeft == true || _cutLogic.CurrentCutter.IsActiveHullOnRight == true) && IsActiveHullApproachCompleted == false;
        }

        private bool IsRotationCompleted()
        {
            var diff = Quaternion.Angle(transform.rotation, _targetRotation);
            return diff <= angleThreshold;
        }
        
        private void HandleNextPlatformAlignment()
        {
            _targetRotation = Quaternion.Euler(0f, targetY, 0f);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                _targetRotation,
                Time.deltaTime * RotationSpeed
            );
            
            if (IsRotationCompleted())
            {
                transform.rotation = _targetRotation;
                IsActiveHullApproachCompleted = false;
                _cutLogic.CurrentCutter.IsActiveHullOnLeft = false;
                _cutLogic.CurrentCutter.IsActiveHullOnRight = false;
            }
        }
        

        private void HandleApproach()
        {
            Direction = _cutLogic.CurrentCutter.ActiveHullDownCenterLocation - transform.position;
            
            Angle = SMath.AngleBetweenSpecificLocations(_cutLogic.CurrentCutter.ActiveHullDownCenterLocation, transform.position);
            
            var targetRotation = Quaternion.Euler(0f, Angle, 0f);
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * RotationSpeed
            );

            CheckDistanceToTarget();
        }
        
        private void CheckDistanceToTarget()
        {
            Distance = Vector3.Distance(_cutLogic.CurrentCutter.ActiveHullDownCenterLocation, transform.position);
            MovementDirection = transform.forward + Direction;
            if (Distance < distanceThreshold)
            {
                IsActiveHullApproachCompleted = true;
                targetY = transform.eulerAngles.y - Angle;
                
                MovementDirection = _platformTracker.CurrentPlatform.GetTransform().forward;
            }
        }

        private void OnToggleMovement(MovementToggleSignal signal)
        {
            if (signal.Enable) EnableMovement();
            else DisableMovement();
        }
        
        private void EnableMovement()
        {
            _playerEntity.IsMovementEnable = true;
        }

        private void DisableMovement()
        {
            _playerEntity.IsMovementEnable = false;
        }
    }
}