using UnityEngine;

namespace Source.Data.Entities
{
    public interface IPlatformData
    {
        bool IsSpawnedRight { get; set; }
        Transform GetTransform();
        Renderer GetRenderer();
    }
}