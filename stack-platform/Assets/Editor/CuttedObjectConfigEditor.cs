using Source.Data.Cut;
using UnityEditor;
using UnityEngine;

namespace Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CuttedObjectConfig))]
    public class CuttedObjectConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSection("LEFT HULL", serializedObject.FindProperty("LeftHull"),
                new Color32(150, 80, 90, 255)); 
            GUILayout.Space(10);
            DrawSection("RIGHT HULL", serializedObject.FindProperty("RightHull"),
                new Color32(150, 150, 90, 255));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSection(string title, SerializedProperty hullProp, Color backgroundColor)
        {
            var backgroundStyle = new GUIStyle(GUI.skin.box)
            {
                normal =
                {
                    background = MakeTex(1, 1, backgroundColor)
                }
            };

            var titleStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.black }
            };

            EditorGUILayout.BeginVertical(backgroundStyle);
            GUILayout.Label(title, titleStyle);

            DrawHull(hullProp);

            EditorGUILayout.EndVertical();
        }

        private void DrawHull(SerializedProperty hullProp)
        {
            DrawColoredProperty(hullProp.FindPropertyRelative("LeftLocation"), Color.white);
            DrawColoredProperty(hullProp.FindPropertyRelative("RightLocation"), Color.white);
            DrawColoredProperty(hullProp.FindPropertyRelative("Width"), Color.white);
        }

        private void DrawColoredProperty(SerializedProperty property, Color labelColor)
        {
            var rect = EditorGUILayout.GetControlRect();

            var labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 14,
                normal =
                {
                    textColor = labelColor
                },
                fontStyle = FontStyle.Italic
            };

            // Sadece label rengi değiştiriyoruz
            EditorGUI.LabelField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, rect.height),
                property.displayName, labelStyle);

            // Değer kutusunu bozmayıp çiziyoruz
            EditorGUI.PropertyField(
                new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y, rect.width - EditorGUIUtility.labelWidth, rect.height),
                property, GUIContent.none, true
            );
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
#endif
}
