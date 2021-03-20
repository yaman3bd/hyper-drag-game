using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadGenerator))]
public class EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoadGenerator levelGenerator = (RoadGenerator)target;

        if (GUILayout.Button("Generate"))
            levelGenerator.GenerateRoad();

        
    }
}
