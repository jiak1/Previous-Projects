using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlocks : MonoBehaviour
{

    public List<Block> gameBlocks = new List<Block>();
    public List<string> gameBlockStrings = new List<string>();
    // Use this for initialization
    void Start()
    {
        GetAllBlocks();
        GenerateLevelEditor();
    }

    void GetAllBlocks()
    {
        gameBlocks.Clear();
        Debug.Log("Generating Game Blocks List From File...");
        foreach (GameObject go in Resources.LoadAll("Blocks/"))
        {
            Debug.Log("Found " + go.name + " Adding...");
            Block blockToAdd = new Block();
            blockToAdd.name = go.name;
            blockToAdd.prefab = go;
            blockToAdd.icon = (Texture2D)Resources.Load("Icons/" + go.name);
            gameBlocks.Add(blockToAdd);
            gameBlockStrings.Add(go.name);
            Debug.Log("Added " + go.name);
        }
        Debug.Log("Generating Game Blocks List Complete");
    }

    void GenerateLevelEditor()
    {
        UIManager uiM = gameObject.AddComponent<UIManager>();
        GameObject levelEditorGO = (GameObject)Resources.Load("LevelEditor/LevelController");
        levelEditorGO = GameObject.Instantiate(levelEditorGO);
        uiM.GameBlockDataGenerated();
    }
}
