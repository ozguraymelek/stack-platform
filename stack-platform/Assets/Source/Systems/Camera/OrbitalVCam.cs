using Cinemachine;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Systems.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class OrbitalVCam : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [Tooltip("Rotation Speed as second")]
        public float speed = 30f;
        [SerializeField] private bool canOrbit = false;
        private bool _canLerpBias = false;
        [SerializeField] private float biasLerpSpeed;
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
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
            _signalBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
            _signalBus.Subscribe<GameFailedSignal>(UnbindProperties);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<LevelStartedSignal>(OnLevelStarted);
            _signalBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
            _signalBus.Unsubscribe<GameFailedSignal>(UnbindProperties);
        }
        
        private void Update()
        {
            if (_canLerpBias)
                _orbital.m_Heading.m_Bias = Mathf.Lerp(_orbital.m_Heading.m_Bias, 0f, biasLerpSpeed * Time.deltaTime);
            
            if (canOrbit == false) return;
            
            _angle -= speed * Time.deltaTime;
            if (_angle <= 0f) _angle += 360f;
            _orbital.m_Heading.m_Bias = _angle;
        } 
        
        private void OnGameStarted()
        {
            canOrbit = false;
        }
        
        public void OnLevelStarted()
        {
            Debug.Log("OnLevelStarted");
            canOrbit = false;
            _canLerpBias = true;
            _angle = 0;
        }
        
        private void OnLevelFinished()
        {
            canOrbit = true;
        }

        private void UnbindProperties()
        {
            _vCam.Follow = null;
            _vCam.LookAt = null;
        }
    }
}