using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateGameObjects))]

public class GenerateGameObjectsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GenerateGameObjects genGO = (GenerateGameObjects)target;

        if (GUILayout.Button("Delete Objects"))
        {
            genGO.RemovePreviousObjects();
        }
    }
}
