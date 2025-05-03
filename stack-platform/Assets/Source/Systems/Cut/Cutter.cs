using EzySlice;
using Source.Core.Utilities.External;
using Source.Data.Cut;
using Source.Gameplay.Platform.Wrappers;
using Source.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace Source.Systems.Cut
{
    public class Cutter : MonoBehaviour, ICutter
    {
        public class Factory : PlaceholderFactory<Cutter> { }
        
        public bool IsActiveHullOnLeft { get; set; }
        public bool IsActiveHullOnRight { get; set; }
        public Vector3 ActiveHullDownCenterLocation { get; set; }

        private IObjectPool _platformPool;
        
        public GameObject CurrentRightHull;
        public GameObject CurrentLeftHull;
        
        private CuttedObjectConfig _cuttedObjectConfig;

        [Inject]
        public void Construct(IObjectPool objectPooling, CuttedObjectConfig cuttedObjectConfig)
        {
            _platformPool = objectPooling;
            _cuttedObjectConfig = cuttedObjectConfig;
            
            SLog.InjectionStatus(this,
                (nameof(_platformPool), _platformPool),
                (nameof(_cuttedObjectConfig), _cuttedObjectConfig)
            );
        }
        
        public void ExternalCut(Transform objectWillSlice, IInteractable<Source.Gameplay.Platform.Platform> objectToSlice, FellHullSide side,
            Material crossSectionMat = null)
        {
            var cutted = InternalCut(objectWillSlice, objectToSlice.GetTransform().gameObject, crossSectionMat);
            CurrentLeftHull = cutted.CreateUpperHull(objectToSlice.GetTransform().gameObject, crossSectionMat);
            _cuttedObjectConfig.Set(ref CurrentLeftHull, ref _cuttedObjectConfig.LeftHull);
            
            CurrentRightHull = cutted.CreateLowerHull(objectToSlice.GetTransform().gameObject, crossSectionMat);
            _cuttedObjectConfig.Set(ref CurrentRightHull, ref _cuttedObjectConfig.RightHull);
            
            switch (side)
            {
                case FellHullSide.Left:
                    CurrentRightHull.layer = LayerMask.NameToLayer("Platform");
                    _cuttedObjectConfig.SetFallComponents(CurrentLeftHull);
                    _cuttedObjectConfig.SetActiveComponents(CurrentRightHull, ref _cuttedObjectConfig.RightHull);
                    ActiveHullDownCenterLocation = STransform.GetBottomCenter(CurrentRightHull.gameObject);
                    IsActiveHullOnLeft = false;
                    IsActiveHullOnRight = true;
                    break;
                case FellHullSide.Right:
                    CurrentLeftHull.layer = LayerMask.NameToLayer("Platform");
                    _cuttedObjectConfig.SetFallComponents(CurrentRightHull);
                    _cuttedObjectConfig.SetActiveComponents(CurrentLeftHull, ref _cuttedObjectConfig.LeftHull);
                    ActiveHullDownCenterLocation = STransform.GetBottomCenter(CurrentLeftHull.gameObject);
                    IsActiveHullOnRight = false;
                    IsActiveHullOnLeft = true;
                    break;
            }
            
            objectWillSlice.gameObject.SetActive(false);
            _platformPool.Release(objectToSlice.GetReference());
        }

        public Transform GetTransform()
        {
            return transform;
        }
        
        private SlicedHull InternalCut(Transform objectWillSlice, GameObject objectToSlice, Material crossSectionMat = null)
        {
            return objectToSlice.Slice(objectWillSlice.position, objectWillSlice.up, crossSectionMat);
        }
    }
    
    public enum FellHullSide
    {
        Left,
        Right
    }
}