using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Source.Systems.Camera
{
    public class BlendCallbacks : MonoBehaviour
    {
        private CinemachineVirtualCamera _vcamBase;

        [Serializable] public class BlendFinishedEvent : UnityEvent<CinemachineVirtualCamera> {}
        public BlendFinishedEvent OnBlendFinished;

        private void Start()
        {
            _vcamBase = GetComponent<CinemachineVirtualCamera>();
            ConnectToVcam(true);
            enabled = false;
        }

        private void ConnectToVcam(bool connect)
        {
            var vcam = _vcamBase as CinemachineVirtualCamera;
            if (vcam != null)
            {
                vcam.m_Transitions.m_OnCameraLive.RemoveListener(OnCameraLive);
                if (connect)
                    vcam.m_Transitions.m_OnCameraLive.AddListener(OnCameraLive);
            }
        }

        private void OnCameraLive(ICinemachineCamera vcamIn, ICinemachineCamera vcamOut)
        {
            enabled = true;
        }

        private void Update()
        {
            var brain = CinemachineCore.Instance.FindPotentialTargetBrain(_vcamBase);
            if (brain == null)
                enabled = false;
            else if (!brain.IsBlending)
            {
                if (brain.IsLive(_vcamBase))
                    OnBlendFinished.Invoke(_vcamBase);
                enabled = false;
            }
        }
    }
}
