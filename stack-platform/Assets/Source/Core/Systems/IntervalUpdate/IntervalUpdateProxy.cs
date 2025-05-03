using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Source.Core.Systems.IntervalUpdate
{
    [DisallowMultipleComponent]
    public class IntervalUpdateProxy : MonoBehaviour, IIntervalUpdate
    {
        [Header("Interval Settings")]
        public IntervalMode Mode = IntervalMode.TimeBased;
        [Tooltip("Number of frames between calls if Mode is Frame-Based")]
        public int FrameInterval = 10;
        [Tooltip("Number of seconds between calls if Mode is Time-Based")]
        public float TimeInterval = 0.5f;

        [Header("Tick Event")]
        public UnityEvent OnIntervalUpdate;

        IntervalMode IIntervalUpdate.Mode => Mode;
        int IIntervalUpdate.FrameInterval => FrameInterval;
        float IIntervalUpdate.TimeInterval => TimeInterval;

        void IIntervalUpdate.IntervalUpdate()
        {
            OnIntervalUpdate?.Invoke();
        }

        private void OnEnable()
        {
            IntervalUpdateManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            IntervalUpdateManager.Instance.Unregister(this);
        }
    }
}