using Source.Core.Utilities.External;
using Source.Data.Sound;
using Source.Infrastructure.Signals;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Source.Systems.GameFlow
{
    public class SoundManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        public SoundData SoundData;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float currentPitch;
        [SerializeField] private AudioClip clip;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus)
            );
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<StreakSignal>(PlaySound);
            _signalBus.Subscribe<StreakLostSignal>(ResetPitch);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<StreakSignal>(PlaySound);
            _signalBus.Unsubscribe<StreakLostSignal>(ResetPitch);
        }

        private void Pitch(int streak)
        {
            currentPitch = SoundData.BasePitch + SoundData.PitchIncreaseStep * streak;
        }
        
        private void PlaySound(StreakSignal signal)
        {
            Pitch(signal.Streak);
            audioSource.pitch = currentPitch;
            audioSource.PlayOneShot(clip);
        }
        
        private void ResetPitch()
        {
            currentPitch = SoundData.BasePitch;
        }
    }
}