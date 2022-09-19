using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(gun))]
public class gunUnityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);


        //gun myTarget = (gun)target;

        //myTarget.gunType = EditorGUILayout.EnumFlagsField("Gun Type", 
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}
