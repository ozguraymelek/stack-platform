namespace Source.Infrastructure.Signals
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