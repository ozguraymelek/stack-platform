using UnityEngine;

namespace _Project.Layers.Game_Logic.Sound
{
    [CreateAssetMenu(fileName = "Sound Data", menuName = "Data/Sound/Create sound data", order = 1)]
    public class SoundData : ScriptableObject
    {
        public float BasePitch;
        public float PitchIncreaseStep;
    }
}
