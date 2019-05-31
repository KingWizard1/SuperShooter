//using UnityEditor;
//using UnityEngine;

//[CanEditMultipleObjects]
//[CustomEditor(typeof(AnalogueGlitch))]
//public class AnalogueGlitchEditor : Editor
//{

//    SerializedProperty ScanLineJitter;
//    SerializedProperty VerticalJump;
//    SerializedProperty HorizontalShake;
//    SerializedProperty ColourDrift;

//    ///////////////////////////////////////////////////////////
//    ///////////////////////////////////////////////////////////

//    private void OnEnable()
//    {

//        ScanLineJitter = serializedObject.FindProperty(nameof(AnalogueGlitch._ScanLineJitter));
//        VerticalJump = serializedObject.FindProperty(nameof(AnalogueGlitch._VerticalJump));
//        HorizontalShake = serializedObject.FindProperty(nameof(AnalogueGlitch._HorizontalShake));
//        ColourDrift = serializedObject.FindProperty(nameof(AnalogueGlitch._ColourDrift));

//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        EditorGUILayout.PropertyField(ScanLineJitter);
//        EditorGUILayout.PropertyField(VerticalJump);
//        EditorGUILayout.PropertyField(HorizontalShake);
//        EditorGUILayout.PropertyField(ColourDrift);
//        serializedObject.ApplyModifiedProperties();
//    }

//    ///////////////////////////////////////////////////////////
//    ///////////////////////////////////////////////////////////


//    ///////////////////////////////////////////////////////////
//    ///////////////////////////////////////////////////////////


//    ///////////////////////////////////////////////////////////
//    ///////////////////////////////////////////////////////////


//    ///////////////////////////////////////////////////////////
//    ///////////////////////////////////////////////////////////


//}
