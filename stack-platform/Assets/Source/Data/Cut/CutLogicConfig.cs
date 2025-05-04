using System;
using UnityEngine;

namespace Source.Data.Cut
{
    [CreateAssetMenu(menuName = "Cut/Configs/Logic")]
    public class CutLogicConfig : ScriptableObject
    {
        public CurrentPlatformCutLogicData CurrentPlatform;
        public NextPlatformCutLogicData NextPlatform;
        public float AlignmentToleranceBoundRight;
        public float AlignmentToleranceBoundLeft;

        public bool ShowVisualization;
    }
    
    [Serializable]
    public struct CurrentPlatformCutLogicData
    {
        public AngleData Angle;
        public LocationData Location;
    }
    
    [Serializable]
    public struct NextPlatformCutLogicData
    {
        public AngleData Angle;
        public LocationData Location;
    }
    
    [Serializable]
    public struct AngleData
    {
        public float WithForwardLeft;
        public float WithForwardRight;
        
        public float WithBackwardLeft;
        public float WithBackwardRight;
    }
    
    [Serializable]
    public struct LocationData
    {
        public Vector3 ForwardLeft;
        public Vector3 ForwardRight;
        
        public Vector3 BackwardLeft;
        public Vector3 BackwardRight;
    }
}