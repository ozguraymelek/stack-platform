using System;
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

            Debug.Log($"[SoundManager] Construct called! " +
                      $"SignalBus is {(_signalBus == null ? "NULL" : "OK")}");
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<OnStreak>(PlaySound);
            _signalBus.Subscribe<OnStreakLost>(ResetPitch);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<OnStreak>(PlaySound);
            _signalBus.Unsubscribe<OnStreakLost>(ResetPitch);
        }

        private void Pitch(int streak)
        {
            CurrentPitch = SoundData.BasePitch + SoundData.PitchIncreaseStep * streak;
        }
        
        private void PlaySound(OnStreak signal)
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