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
        
        private void SetInternalPositionBeforeAssign(ref Vector3 position, ref Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
        
        public void SpawnCutter(Cutter.Factory cutterFactory, Vector3 position, Quaternion rotation, out ICutter spawnedCutter)
        {
            SetInternalPositionBeforeAssign(ref position, ref rotation);
            // spawnedCutter = Instantiate(Prefab, position, rotation).GetComponent<Cutter>();
            if (cutterFactory == null)
            {
                Debug.LogError("CutterFactory is NULL! Injection olmadı.");
                spawnedCutter = null;
                return;
            }
            spawnedCutter = cutterFactory.Create();
            spawnedCutter.GetTransform().position = Position;
            spawnedCutter.GetTransform().rotation = Rotation;
        }
    }
}
