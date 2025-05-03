using System;
using _Project.Helper.Utils;
using _Project.Layers.Data.Entities;
using _Project.Layers.Data.Interfaces.Player;
using _Project.Layers.Game_Logic.Cut;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Inject] private PlayerEntity _playerEntity;
        
        private CutLogic _cutLogic;
        private PlatformTracker _platformTracker;
        
        public Vector3 MovementDirection;
        
        public float MovementSpeed;
        public float RotationSpeed;
        public float Distance;

        public float Angle;
        public Vector3 Direction;
        public float targetYRotation;
        private Quaternion targetRotation2;

        public bool IsActiveHullApproachCompleted = false;
        
        [SerializeField] private float distanceThreshold;
        [SerializeField] private float angleThreshold;
        
        [Inject]
        public void Construct(CutLogic cutLogic, PlatformTracker platformTracker)
        {
            _cutLogic = cutLogic;
            _platformTracker = platformTracker;
            
            Debug.Log($"[PlayerMovement] Construct called! " +
                      $"Cut Logic is {(_cutLogic == null ? "NULL" : "OK")}" +
                      $"Platform Tracker is {(_platformTracker == null ? "NULL" : "OK")}");

        }
        
        private void Update()
        {
            Debug.Log(_playerEntity.IsMovementEnable);
            if (_playerEntity.IsMovementEnable == false)
            {
                MovementDirection = Vector3.zero;
                return;
            }
            
            if (TryApproachToActiveHull()) HandleApproach();
            else if (IsActiveHullApproachCompleted == true) HandleNextPlatformAlignment();
            else MovementDirection = Vector3.forward;
            
            transform.position += MovementDirection.normalized * (Time.deltaTime * MovementSpeed);
            
            Debug.Log("Character moving");
        }
        
        private bool TryApproachToActiveHull()
        {
            if (_cutLogic.CurrentCutter == null) return false;
            return (_cutLogic.CurrentCutter.IsActiveHullOnLeft == true || _cutLogic.CurrentCutter.IsActiveHullOnRight == true) && IsActiveHullApproachCompleted == false;
        }

        private bool IsRotationCompleted()
        {
            var diff = Quaternion.Angle(transform.rotation, targetRotation2);
            return diff <= angleThreshold;
        }
        
        private void HandleNextPlatformAlignment()
        {
            targetRotation2 = Quaternion.Euler(0f, targetYRotation, 0f);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation2,
                Time.deltaTime * RotationSpeed
            );
            
            if (IsRotationCompleted())
            {
                transform.rotation = targetRotation2;
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
                targetYRotation = transform.eulerAngles.y - Angle;
                
                MovementDirection = _platformTracker.CurrentPlatform.GetTransform().forward;
                
                // PlatformSpawner.OnSpawnPlatform?.Invoke();
                Debug.Log("OnSpawnPlatform");
            }
        }
        
        //called from animation events
        public void EnableMovement()
        {
            _playerEntity.IsMovementEnable = true;
        }

        //called from animation events
        public void DisableMovement()
        {
            _playerEntity.IsMovementEnable = false;
        }
    }
}