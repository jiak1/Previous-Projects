using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapController))]
public class NoisePreviewEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MapController mapController = (MapController)target;
        if (DrawDefaultInspector())
        {
            if (mapController.autoUpdate)
            {
                mapController.GenerateNoiseMap();
            }
        }



        if (GUILayout.Button("Generate"))
        {
            mapController.GenerateNoiseMap();
        }
    }
}
