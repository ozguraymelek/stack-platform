using UnityEngine;

namespace Source.Data.Sound
{
    [CreateAssetMenu(fileName = "Sound Data", menuName = "Data/Sound/Create a sound data", order = 1)]
    public class SoundData : ScriptableObject
    {
        public float BasePitch;
        public float PitchIncreaseStep;
    }
}