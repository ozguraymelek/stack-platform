using UnityEngine;

namespace _Project.Layers.Game_Logic.Cut
{
    // 1. Define the cutter interface
    public interface ICutter
    {
        // Executes a cut operation on the given target
        void Cut(GameObject target);
        Transform GetTransform();
    }
}