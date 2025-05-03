namespace Source.Core.Systems.IntervalUpdate
{
    public enum IntervalMode
    {
        FrameBased,
        TimeBased
    }
    
    public interface IIntervalUpdate 
    {
        void IntervalUpdate();
        IntervalMode Mode { get; }
        int FrameInterval { get; }
        float TimeInterval { get; }
    }
}