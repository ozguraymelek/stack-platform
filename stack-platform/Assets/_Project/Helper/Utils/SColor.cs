using UnityEngine;

namespace _Project.Helper.Utils
{
    public static class SColor
    {
        public static Color RandomColorHSVToRGB()
        {
            return Color.HSVToRGB(Random.value, 0.5f, 1f);
        }
    }
}
