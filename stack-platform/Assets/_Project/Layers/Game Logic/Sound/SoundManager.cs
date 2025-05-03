using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Sound
{
    public class SoundManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        public SoundData SoundData;
        public AudioSource AudioSource;
        public float CurrentPitch;
        public AudioClip Clip;
        
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
            CurrentPitch = SoundData.BasePitch + SoundData.PitchIncreaseStep * streak;
        }
        
        private void PlaySound(StreakSignal signal)
        {
            Pitch(signal.Streak);
            AudioSource.pitch = CurrentPitch;
            AudioSource.PlayOneShot(Clip);
        }
        
        private void ResetPitch()
        {
            CurrentPitch = SoundData.BasePitch;
        }
    }
}