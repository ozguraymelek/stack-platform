using System.Text;
using UnityEditor;
using UnityEngine;

namespace _Project.Helper.Utils
{
    public class SLog
    {
        private static bool _isInjectionStatusLogActive;
        
        /// <summary>
        /// Visibility on the console can be adjusted with "Log/Injection Status" in the Editor menu.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="injections"></param>
        public static void InjectionStatus(object caller, params (string name, object value)[] injections)
        {
            if (_isInjectionStatusLogActive == false) return;
            var sb = new StringBuilder();
            
            sb.AppendFormat("<b><color=#E9F095>[{0}]</color></b> Construct called! ---> ", caller.GetType().Name);

            var anyNull = false;
            foreach (var (name, value) in injections)
            {
                var isNull = value == null;
                if (isNull) anyNull = true;
                var color = isNull ? "#FF8888" : "#88FF88";
                var status = isNull ? "NULL" : "INJECTED";
                sb.AppendFormat("<b>[{0}: <color={1}>{2}</color>]</b> ", name, color, status);
            }

            if (!anyNull)
                Debug.LogWarning(sb.ToString());
            else
                Debug.LogError(sb.ToString());
            
        }
#if UNITY_EDITOR
        [MenuItem("Log/Injection Status")]
        public static void LogToggle()
        {
            _isInjectionStatusLogActive = !_isInjectionStatusLogActive;
            Debug.LogWarning(_isInjectionStatusLogActive
                ? "Injection Status logs enabled"
                : "Injection Status logs disabled");
        }
#endif
    }
}
