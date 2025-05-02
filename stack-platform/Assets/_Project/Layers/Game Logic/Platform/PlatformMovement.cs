using System;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Platform
{
    public class PlatformMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        private bool _isMoving = true;
        public Vector3 MovementDirection = Vector3.right;
        
        private SignalBus _signalBus;
        private IPlatformData _currentPlatformData;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _currentPlatformData = GetComponent<Platform>();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlatformStopRequestedSignal>(OnStopRequested);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlatformStopRequestedSignal>(OnStopRequested);
        }

        private void Update()
        {
            if (_isMoving == false) return;

            // Debug.Log($"spawned right: {_currentPlatformData.IsSpawnedRight}");
            transform.position += new Vector3(
                (_currentPlatformData.IsSpawnedRight ? -MovementDirection.x : MovementDirection.x) * moveSpeed *
                Time.deltaTime, 0, 0);
        }
        
        
        public void OnInstantiated(IPlatformData platformData)
        {
            _currentPlatformData = platformData;
        }
        
        private void OnStopRequested()
        {
            _isMoving = false;
        }
    }
}
