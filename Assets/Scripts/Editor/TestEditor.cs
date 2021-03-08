using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RoadGenerator))]
public class RoadGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoadGenerator tempTransform = (RoadGenerator)target;

        if (GUILayout.Button("Fix Cars"))
            tempTransform.GenerateRoad();


    }
}


[CustomEditor(typeof(InGameCarsInitializer))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InGameCarsInitializer tempTransform = (InGameCarsInitializer)target;

        if (GUILayout.Button("Fix Cars"))
            tempTransform.InitializeCars();
        if (GUILayout.Button("Fix Cars Body"))

            tempTransform.InitializeCarsBody();

    }
}

[CustomEditor(typeof(CarsDataScriptableObjectInitializer))]
public class TestscriptEdito : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CarsDataScriptableObjectInitializer tempTransform = (CarsDataScriptableObjectInitializer)target;

        if (GUILayout.Button("Fix Cars"))
            tempTransform.InitializeCarsData();

    }
}
