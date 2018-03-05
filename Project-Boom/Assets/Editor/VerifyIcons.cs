using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.IO;

public class VerifyIcons : EditorWindow
{

    List<string> currentIcons = new List<string>();
    List<string> newIcons = new List<string>();
    int numOfBlocks = 0;
    bool generatingIcons = false;
    int amountToGen = 0;
    [MenuItem("Window/Verify Icons")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<VerifyIcons>("Verify Icons Window");
    }

    private void OnGUI()
    {

        if (generatingIcons)
        {
            GUILayout.Label("GENERATING ICONS PLEASE WAIT......", EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Label("Click Below To Verify And Fix All Items: ", EditorStyles.boldLabel);
            if (GUILayout.Button("Verify Icons"))
            {
                if (generatingIcons)
                {
                    Debug.Log("Already Generating Icons, Please Wait Until Finished...");
                }
                else
                {
                    amountToGen = 0;
                    numOfBlocks = 0;
                    Debug.Log("Verifying Icons...");
                    currentIcons.Clear();

                    foreach (Texture2D t2D in Resources.LoadAll("Icons/"))
                    {
                        currentIcons.Add(t2D.name);
                        GUIContent content = new GUIContent();
                        content.image = t2D;

                        content.text = "Found " +t2D.name + ", Press To Delete Icon";
                        content.tooltip = "Press To Delete " + t2D.name+" Icon";

                        Vector2 scrollPos = new Vector2();
                        EditorGUILayout.BeginHorizontal();
                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
                        
                        EditorGUILayout.EndScrollView();
                        if (GUILayout.Button(content, GUILayout.Width(100), GUILayout.Height(100)))
                        {
                            Debug.Log("Deleting " + content.image.name + " Icon.");
                            EditorGUILayout.EndHorizontal();
                        }
                        if (GUILayout.Button("Clear All"))
                        {
                            Debug.Log("Clearing All Cached Icons");
                        }
                    }
                    foreach (GameObject go in Resources.LoadAll("Blocks/"))
                    {
                        if (!(currentIcons.Contains(go.name)))
                        {
                            amountToGen = amountToGen + 1;
                        }
                    }
                    AssetPreview.SetPreviewTextureCacheSize(amountToGen);
                    foreach (GameObject go in Resources.LoadAll("Blocks/"))
                    {
                        numOfBlocks = numOfBlocks + 1;
                        if (currentIcons.Contains(go.name))
                        {
                            Debug.Log("Ignoring " + go.name + " Icon Already Exists...");
                        }
                        else
                        {
                            Debug.Log("Need Icon For " + go.name + " Generating...");
                            Texture2D t2D = null;
                            int tries = 0;
                            while (AssetPreview.IsLoadingAssetPreview(go.GetInstanceID()))
                            {
                                t2D = AssetPreview.GetAssetPreview(go);
                                Thread.Sleep(100);

                                tries = tries + 1;
                            }
                            t2D = AssetPreview.GetAssetPreview(go);
                            if (t2D != null)
                            {
                                byte[] bytes = t2D.EncodeToPNG();
                                File.WriteAllBytes(Application.dataPath + "/Resources/Icons/" + go.name + ".png", bytes);
                                generatingIcons = true;
                            }
                        }
                    }
                }
            }


        }

        foreach (Texture2D t2D in Resources.LoadAll("Icons/"))
        {
            GUILayout.Label("Found Icon " + t2D.name + ": ");
            GUILayout.Label(t2D);
        }

    }
   
    private void OnInspectorUpdate()
    {
        if (generatingIcons)
        {
            newIcons.Clear();

            foreach (Texture2D t2D in Resources.LoadAll("Icons/"))
            {
                newIcons.Add(t2D.name);
            }
            if (newIcons.Count == numOfBlocks)
            {
                Debug.Log("Finished Generating Icons");
                generatingIcons = false;
            }
        }
    }
}
