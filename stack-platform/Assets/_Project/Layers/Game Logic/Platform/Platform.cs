using _Project.Layers.Data.Entities;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public enum PlatformSide
    {
        Left,
        Right
    }
    public class Platform : MonoBehaviour, IInteractable, IPlatformData
    {
        public bool IsSpawnedRight { get; set; }
        public Renderer Renderer;

        public Vector3 GetNextSpawnPosition(Transform transform, float platformLength)
        {
            // Örneğin bir sonraki platform spawn pozisyonunu hesaplayacak
            return transform.position + new Vector3(0, 0, platformLength);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public Platform GetReference()
        {
            return this;
        }

        public Renderer GetRenderer()
        {
            return Renderer;
        }
    }
}
