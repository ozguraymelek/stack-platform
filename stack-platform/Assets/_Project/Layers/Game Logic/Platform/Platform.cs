using System;
using _Project.Layers.Data.Entities;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    public enum PlatformSide
    {
        Left,
        Right
    }
    public class Platform : MonoBehaviour, IInteractable<Platform>, IPlatformData
    {
        public bool IsSpawnedRight { get; set; }
        public Renderer Renderer;


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
