using System.Collections.Generic;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Platform
{
    [CreateAssetMenu(fileName = "New Platform Material Data", menuName = "Data/Platform/Create a new platform material data")]
    public class PlatformMaterialData : ScriptableObject
    {
        public List<Material> materials;
    }
}
