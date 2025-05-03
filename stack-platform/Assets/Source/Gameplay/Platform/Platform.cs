using Source.Core.Utilities.External;
using Source.Gameplay.Platform.Wrappers;
using Source.Infrastructure.Pools;
using Source.Systems.Effects;
using UnityEngine;

namespace Source.Gameplay.Platform
{
    public class Platform : MonoBehaviour, IInteractable<Platform>
    {
        private IObjectPool _platformPool;

        public IObjectPool PlatformPool
        {
            get => _platformPool;
            set => _platformPool = value;
        }
        
        public bool IsSpawnedRight { get; set; }
        public EdgeOutline Outline;
        public Renderer Renderer;
        
        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }

        public Transform GetTransform() => transform;

        public Platform GetReference() => this;

        public EdgeOutline GetOutline() => Outline;

        public Renderer GetRenderer() => Renderer;
        
        public void IsObjectOutOfCameraFrustum()
        {
            Debug.LogWarning("TESTT");
            if (SRender.IsObjectOutOfCameraFrustum(Renderer, _camera))
            {
                _platformPool.Release(this);
            }
        }
    }
}