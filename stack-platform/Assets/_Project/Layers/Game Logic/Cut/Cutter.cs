using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Infrastructure.Pools;
using EzySlice;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Cut
{
    // 2. Concrete implementation of the cutter
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
            
            Debug.Log($"[Cutter] Construct called! Config is {(_cuttedObjectConfig == null ? "NULL" : "OK")}");

        }


        public void ExternalCut(Transform objectWillSlice, IInteractable<Platform.Platform> objectToSlice, FellHullSide side,
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
                    _cuttedObjectConfig.SetFallComponents(CurrentLeftHull);
                    _cuttedObjectConfig.SetActiveComponents(CurrentRightHull, ref _cuttedObjectConfig.RightHull);
                    ActiveHullDownCenterLocation = STransform.GetBottomCenter(CurrentRightHull.gameObject);
                    IsActiveHullOnLeft = false;
                    IsActiveHullOnRight = true;
                    break;
                case FellHullSide.Right:
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