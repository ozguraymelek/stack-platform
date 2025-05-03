using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Cut
{
    public interface ICutter
    {
        public bool IsActiveHullOnLeft { get; set; }
        public bool IsActiveHullOnRight { get; set; }
        public Vector3 ActiveHullDownCenterLocation { get; set; }
        
        void ExternalCut(Transform objectWillSlice, IInteractable<Platform.Platform> objectToSlice, FellHullSide side , Material crossSectionMat = null);
        Transform GetTransform();
    }
}