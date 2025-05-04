using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "CutConfigColorCustomizer",menuName = "Data/Editor/Color Customizer/Cut Config")]
    public class CutConfigColorCustomizer : ScriptableObject
    {
        public Color AngleWithUpperLeftColor;
        public Color AngleWithUpperRightColor;
        
        public Color AngleWithDownLeftColor;
        public Color AngleWithDownRightColor;
        
        public Color LocationUpperLeftColor;
        public Color LocationUpperRightColor;
        
        public Color LocationDownLeftColor;
        public Color LocationDownRightColor;
    }
}