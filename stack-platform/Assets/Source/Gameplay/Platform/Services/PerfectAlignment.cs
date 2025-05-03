using Source.Gameplay.Platform.Wrappers;
using UnityEngine;

namespace Source.Gameplay.Platform.Services
{
    public class PerfectAlignment : IAlignment
    {
        public int PerfectIntersectionStreak { get; set; }

        public bool IsTherePerfectAlignment(Vector3 left, Vector3 right, float bound)
        {
            var diff = Mathf.Abs(left.x - right.x);
            return diff < bound;
        }

        public void AlignPlatform(Transform nextPlatform, Transform currentTransform)
        {
            nextPlatform.position = currentTransform.position;
            nextPlatform.position += new Vector3(0f, 0f, currentTransform.localScale.z);
        }
    }
}