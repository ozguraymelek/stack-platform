using System;
using _Project.Helper.Utils;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Cut
{
    [CreateAssetMenu(menuName = "Cutting/Configs/Cutted")]
    public class CuttedObjectConfig : ScriptableObject
    {
        public HullData LeftHull;
        public HullData RightHull;

        public void Set(ref GameObject hull, ref HullData hullData)
        {
            // STransform.AdjustPivotByMesh(hull);
            // (hullData.LeftLocation, hullData.RightLocation) = SMath.AnyObjectLeftAndRightCenterPoints(hull);
        }

        public void SetFallComponents(GameObject obj)
        {
            obj.AddComponent<Rigidbody>();
        }
        public void SetActiveComponents(GameObject obj, ref HullData hullData)
        {
            // hullData.Width.Value = STransform.GetAnyObjectWidth(obj);
            
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<Platform.Platform>();
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
