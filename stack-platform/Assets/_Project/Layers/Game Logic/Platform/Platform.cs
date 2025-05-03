using System;
using _Project.Helper.Utils;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Cut;
using _Project.Layers.Game_Logic.Game_Flow;
using _Project.Layers.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Platform
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
        public Renderer Renderer;
        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
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
