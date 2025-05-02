using System;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Layers.Game_Logic.Cut
{
    [CreateAssetMenu(menuName = "Cutting/Configs/CutTracker")]
    public class CutLogicData : ScriptableObject
    {
        public CurrentPlatformCutLogicData CurrentPlatform;
        public NextPlatformCutLogicData NextPlatform;
        public float AlignmentToleranceBoundRight;
        public float AlignmentToleranceBoundLeft;
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