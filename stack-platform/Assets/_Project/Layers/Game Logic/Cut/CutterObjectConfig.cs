using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Cut
{
    [CreateAssetMenu(menuName = "Cutting/Configs/Cutter")]
    public class CutterObjectConfig : ScriptableObject
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public GameObject Prefab;
        
    }
}
