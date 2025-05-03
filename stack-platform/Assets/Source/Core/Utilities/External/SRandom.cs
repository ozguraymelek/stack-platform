using System.Collections.Generic;
using UnityEngine;

namespace Source.Core.Utilities.External
{
    public class SRandom
    {
        public static Color RandomColorHSVToRGB()
        {
            return Color.HSVToRGB(Random.value, 0.5f, 1f);
        }

        public static Material Material(IList<Material> materials)
        {
            if (materials == null || materials.Count == 0)
            {
                Debug.LogError("SRandom.RandomMaterial: materials list is null or empty.");
                return null;
            }

            var index = Random.Range(0, materials.Count);
            return materials[index];
        }
    }
}