using Cinemachine;
using Source.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Source.Systems.Camera
{
    public class TrackingVCam : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        private CinemachineVirtualCamera _vCam;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<GameFailedSignal>(UnbindFollowProperty);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameFailedSignal>(UnbindFollowProperty);
        }
        
        private void UnbindFollowProperty()
        {
            _vCam.Follow = null;
        }
    }
}
