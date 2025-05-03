using Source.Gameplay.Platform;
using Source.Gameplay.Platform.Wrappers;
using UnityEngine;

namespace Source.Systems.Cut
{
    public interface ICutter
    {
        public bool IsActiveHullOnLeft { get; set; }
        public bool IsActiveHullOnRight { get; set; }
        public Vector3 ActiveHullDownCenterLocation { get; set; }
        
        void ExternalCut(Transform objectWillSlice, IInteractable<Platform> objectToSlice, FellHullSide side , Material crossSectionMat = null);
        Transform GetTransform();
    }
}