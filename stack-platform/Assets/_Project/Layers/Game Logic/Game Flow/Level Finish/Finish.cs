using _Project.Layers.Game_Logic.Effect;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Game_Flow.Level_Finish
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