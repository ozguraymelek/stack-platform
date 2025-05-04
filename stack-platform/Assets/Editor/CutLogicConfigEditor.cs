using Source.Data.Cut;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CutLogicConfig))]
    public class CutLogicConfigEditor : UnityEditor.Editor
    {
        private CutConfigColorCustomizer _customizer;
        
        private void OnEnable()
        {
            _customizer = Resources.Load<CutConfigColorCustomizer>("CutConfigColorCustomizer");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSection("CURRENT PLATFORM", serializedObject.FindProperty("CurrentPlatform"),
                Color.black);
            
            GUILayout.Space(10);
            
            DrawSection("NEXT PLATFORM", serializedObject.FindProperty("NextPlatform"),
                Color.black);
            
            GUILayout.Space(10);
            
            DrawSection("SETTINGS", serializedObject.FindProperty("AlignmentToleranceBoundRight"),
                Color.gray, true, true);
            
            DrawSection("SETTINGS", serializedObject.FindProperty("AlignmentToleranceBoundLeft"),
                Color.gray, true, true);
            
            GUILayout.Space(15);
            
            DrawSection("DEBUG", serializedObject.FindProperty("ShowVisualization"),
                Color.gray, true);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSection(string title, SerializedProperty platformProp, Color backgroundColor, bool isPrimitive = false, bool wantSlider = false)
        {
            var backgroundStyle = new GUIStyle(GUI.skin.box)
            {
                normal =
                {
                    background = MakeTex(1, 1, backgroundColor)
                }
            };

            var guiStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.white
                }
            };
            
            EditorGUILayout.BeginVertical(backgroundStyle);
            GUILayout.Label(title, guiStyle);

            if (isPrimitive == false)
                DrawStruct(platformProp);
            else
                DrawPrimitive(platformProp, wantSlider);

            EditorGUILayout.EndVertical();
        }

        private void DrawPrimitive(SerializedProperty primitiveProp, bool wantSlider = false)
        {
            GUILayout.Space(5);
            DrawColoredProperty(primitiveProp, Color.black);
        }
        
        private void DrawStruct(SerializedProperty structProp)
        {
            var angleProp = structProp.FindPropertyRelative("Angle");
            var locationProp = structProp.FindPropertyRelative("Location");
            
            var labelStyle = new GUIStyle(EditorStyles.label)
            {
                normal =
                {
                    textColor = Color.white
                },
                fontStyle = FontStyle.Bold,
                fontSize = 22
            };

            GUILayout.Space(5);
            GUILayout.Label("ANGLE", labelStyle);
            DrawColoredProperty(angleProp.FindPropertyRelative("WithForwardLeft"), _customizer.AngleWithUpperLeftColor);
            DrawColoredProperty(angleProp.FindPropertyRelative("WithForwardRight"), _customizer.AngleWithUpperRightColor);
            DrawColoredProperty(angleProp.FindPropertyRelative("WithBackwardLeft"), _customizer.AngleWithDownLeftColor);
            DrawColoredProperty(angleProp.FindPropertyRelative("WithBackwardRight"), _customizer.AngleWithDownRightColor);

            GUILayout.Space(5);
            GUILayout.Label("LOCATION", labelStyle);
            DrawColoredProperty(locationProp.FindPropertyRelative("ForwardLeft"), _customizer.LocationUpperLeftColor);
            DrawColoredProperty(locationProp.FindPropertyRelative("ForwardRight"), _customizer.LocationUpperRightColor);
            DrawColoredProperty(locationProp.FindPropertyRelative("BackwardLeft"), _customizer.LocationDownLeftColor);
            DrawColoredProperty(locationProp.FindPropertyRelative("BackwardRight"), _customizer.LocationDownRightColor);
            
        }

        private void DrawColoredProperty(SerializedProperty property, Color labelColor)
        {
            var rect = EditorGUILayout.GetControlRect();
            var labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Italic,
                normal =
                {
                    textColor = labelColor
                },
                
            };

            EditorGUI.LabelField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, rect.height), property.displayName, labelStyle);
            EditorGUI.PropertyField(new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y, rect.width - EditorGUIUtility.labelWidth, rect.height), property, GUIContent.none, true);
        }
        
        private Texture2D MakeTex(int width, int height, Color col)
        {
            var pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}