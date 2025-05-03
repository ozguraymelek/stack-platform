using UnityEngine;

namespace _Project.Helper.Utils.API
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
        // bool UseFrameBased { get; }
        int FrameInterval { get; }
        float TimeInterval { get; }
    }
}
