using System;
using Source.Core.Utilities.External;
using UnityEngine;

namespace Source.Data.Cut
{
    [CreateAssetMenu(menuName = "Cut/Configs/Cutted")]
    public class CuttedObjectConfig : ScriptableObject
    {
        public HullData LeftHull;
        public HullData RightHull;

        public void Set(ref GameObject hull, ref HullData hullData)
        {
            STransform.AdjustPivotByMesh(hull);
            (hullData.LeftLocation, hullData.RightLocation) = SMath.AnyObjectLeftAndRightCenterPoints(hull);
        }

        public void SetFallComponents(GameObject obj)
        {
            obj.AddComponent<Rigidbody>();
        }
        public void SetActiveComponents(GameObject obj, ref HullData hullData)
        {
            hullData.Width = STransform.GetAnyObjectWidth(obj);
            
            obj.AddComponent<BoxCollider>();
            var platform = obj.AddComponent<Gameplay.Platform.Platform>();
            platform.Renderer = platform.GetComponent<Renderer>();
        }
    }
    
    [Serializable]
    public struct HullData
    {
        public Vector3 LeftLocation;
        public Vector3 RightLocation;
        public float Width;
    }
}