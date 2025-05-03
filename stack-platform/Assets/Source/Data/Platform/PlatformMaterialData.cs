using System.Collections.Generic;
using UnityEngine;

namespace Source.Data.Platform
{
    [CreateAssetMenu(fileName = "Platform Material Data", menuName = "Data/Platform/Create a new platform material data")]
    public class PlatformMaterialData : ScriptableObject
    {
        public List<Material> materials;
    }
}