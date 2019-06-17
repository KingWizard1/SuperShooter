//using UnityEditor;
//using UnityEngine;

//namespace SuperShooter
//{
//    /// <summary>Makes a public field read-only when viewed in the inspector.</summary>
//    public class ReadOnlyAttribute : PropertyAttribute
//    {

//    }

//    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
//    public class ReadOnlyDrawer : PropertyDrawer
//    {
//        public override float GetPropertyHeight(SerializedProperty property,
//                                                GUIContent label)
//        {
//            return EditorGUI.GetPropertyHeight(property, label, true);
//        }

//        public override void OnGUI(Rect position,
//                                   SerializedProperty property,
//                                   GUIContent label)
//        {
//            GUI.enabled = false;
//            EditorGUI.PropertyField(position, property, label, true);
//            GUI.enabled = true;
//        }
//    }
//}
