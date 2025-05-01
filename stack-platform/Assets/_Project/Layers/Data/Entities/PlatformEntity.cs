using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Data.Entities
{
    public interface IPlatformData
    {
        bool IsSpawnedRight { get; set; }
        Transform GetTransform();
        Platform GetReference();
        Renderer GetRenderer();
    }
}
