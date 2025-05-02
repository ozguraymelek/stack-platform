using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public interface IAlignment
    {
        public int PerfectIntersectionStreak { get; set; }

        bool IsTherePerfectAlignment(Vector3 left, Vector3 right, float bound);
        void AlignPlatform(Transform nextPlatform, Transform currentTransform);
    }
}
