using UnityEngine;

namespace _Project.Layers.Game_Logic.Signals
{
    public class StreakSignal
    {
        public int Streak { get; }

        public StreakSignal(int streak)
        {
            Streak = streak;
        }
    }
}
