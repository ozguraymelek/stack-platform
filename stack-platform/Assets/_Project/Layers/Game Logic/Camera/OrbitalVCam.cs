using System;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Game_Logic.Signals;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class OrbitalVCam : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [Tooltip("Rotation Speed as second")]
        public float speed = 30f;
        [SerializeField] private bool canOrbit = false;
        private float _angle = 0f;
        
        private CinemachineVirtualCamera _vCam;
        private CinemachineOrbitalTransposer _orbital;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
            _orbital ??= _vCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnLevelStarted);
            _signalBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnLevelStarted);
            _signalBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
        }
        
        private void OnLevelStarted()
        {
            canOrbit = false;
        }
        
        private void OnLevelFinished()
        {
            canOrbit = true;
        }
        
        private void Update()
        {
            if (canOrbit == false) return;
            _angle += speed * Time.deltaTime;
            if (_angle >= 360f) _angle -= 360f;
            _orbital.m_Heading.m_Bias = _angle;
        } 
    }
}