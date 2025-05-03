using Source.Gameplay.Platform.Wrappers;
using Source.Systems.Effects;
using UnityEngine;

namespace Source.Systems.Finish
{
    public class Finish : MonoBehaviour, IInteractable<Finish>
    {
        public Renderer Renderer;
        public EdgeOutline Outline;
        
        public bool IsSpawnedRight { get; set; }

        public Transform GetTransform() => transform;
        public Finish GetReference() => this;
        public EdgeOutline GetOutline() => Outline;
        public Renderer GetRenderer() => Renderer;
    }
}