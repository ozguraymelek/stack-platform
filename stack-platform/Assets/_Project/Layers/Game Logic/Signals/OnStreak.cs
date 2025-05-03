using UnityEngine;

namespace _Project.Layers.Game_Logic.Signals
{
    public class OnStreak
    {
        public int Streak { get; }

        public OnStreak(int streak)
        {
            Streak = streak;
        }
    }
}
