using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class UIManager : MonoBehaviour {

    Block blockToPlace;
    List<Block> gameBlocks;
    public bool genLevelEditorUI = true;
    public bool blockSelectShown = false;
    #region UIVars
    Canvas showBlockCanvas;
    Canvas chooseLevelCanvas;
    Canvas createLevelCanvas;
    Canvas loadLevelCanvas;
    Canvas ColourEditMenuCanvas;
    Canvas maximiseButtonCanvas;
    GameObject buttonPanel;
    Canvas levelEditorMenuCanvas;
    Canvas levelToolCanvas;
    Canvas demolishCanvas;
    List<Button> blockButtons = new List<Button>();
    Canvas confirmCanvas;
    #endregion
    LevelEditor lvE = null;

    public void GameBlockDataGenerated () {
        if (genLevelEditorUI)
        {
            GenChooseLevelEditorUI();
        }
        else
        {
            GenStartGameUI();
        }
	}

    private void Start()
    {
        lvE = GameObject.FindObjectOfType<LevelEditor>();
    }

    void GenChooseLevelEditorUI()
    {

        Debug.Log("Generating Choose Level UI....");
        chooseLevelCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/SelectLevelEditCanvas")).GetComponent<Canvas>();

        GameObject loadButtonGO = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/LoadLevelButton"));
        GameObject createButtonGO = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/CreateLevelButton"));
        GameObject _parent = chooseLevelCanvas.GetComponentInChildren<Image>().gameObject;
        loadButtonGO.transform.SetParent(_parent.transform,false);
        createButtonGO.transform.SetParent(_parent.transform,false);
        
        Button loadButton = loadButtonGO.GetComponent<Button>();
        Button createButton = createButtonGO.GetComponent<Button>();

        loadButton.onClick.AddListener(delegate { LoadLevelButtonPressed(); });
        createButton.onClick.AddListener(delegate { CreateLevelButtonPressed(); });

        Debug.Log("Finished Choose Level UI");
    }

    void LoadLevelButtonPressed()
    {
        Debug.Log("Loading Level Select UI...");
        if (chooseLevelCanvas != null) { Destroy(chooseLevelCanvas.gameObject); }
        loadLevelCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/SelectLoadLevelCanvas")).GetComponent<Canvas>();
        List<string> saves = FileUtil.GetAllSaves();
        foreach (string save in saves)
        {
            Button saveLoadButton = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/Level_Button")).GetComponent<Button>();
            Button deleteLoadButton = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/DeleteLoad_Button")).GetComponent<Button>();
            deleteLoadButton.gameObject.transform.SetParent(saveLoadButton.gameObject.transform, false);
            GameObject parent = loadLevelCanvas.GetComponentInChildren<GridLayoutGroup>().gameObject;
            saveLoadButton.gameObject.transform.SetParent(parent.transform, false);
            saveLoadButton.gameObject.GetComponentInChildren<Text>().text = save;
            saveLoadButton.onClick.AddListener(delegate { LoadLevelByName(save); });
            deleteLoadButton.onClick.AddListener(delegate { ConfirmDeleteLevelByName(save,saveLoadButton.gameObject); });
        }
        Debug.Log("Finished Loading Level Select UI");
    }

    void CreateLevelButtonPressed()
    {
        Destroy(chooseLevelCanvas.gameObject);

        Debug.Log("Generating Create Level UI....");

        createLevelCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/SelectCreateLevelCanvas")).GetComponent<Canvas>();

        GameObject createButtonGO = GameObject.Find("Create_Button");
        GameObject inputCreateName = GameObject.Find("InputField");

        InputField createNameInputField = inputCreateName.GetComponent<InputField>();
        Button createButton = createButtonGO.GetComponent<Button>();
        
        createButton.onClick.AddListener(delegate { LoadEmptyEditLevel(createNameInputField.text); });

        Debug.Log("Finished Generating Create Level UI");

    }

    void LoadLevelByName(string levelName)
    {
        Destroy(loadLevelCanvas.gameObject);
        ShowLevelToolCanvas();
        LevelEditor lvE = GameObject.FindObjectOfType<LevelEditor>();
        lvE.SetBlockCanvasShown(true);
        lvE.LoadOldLevel(levelName);

    }

    void ConfirmDeleteLevelByName(string levelName,GameObject buttonGO)
    {
        confirmCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/ConfirmCanvas")).GetComponent<Canvas>();
        Button noButton = GameObject.Find("NO_BUTTON").GetComponent<Button>();
        Button yesButton = GameObject.Find("YES_BUTTON").GetComponent<Button>();
        yesButton.onClick.AddListener(delegate { DeleteLevelByName(levelName, buttonGO); });
        noButton.onClick.AddListener(delegate { CancelDeleteLevelByName(); });
    }

    void CancelDeleteLevelByName()
    {
        Destroy(confirmCanvas.gameObject);
    }

    void DeleteLevelByName(string levelName, GameObject buttonGO)
    {
        Destroy(confirmCanvas.gameObject);
        Destroy(buttonGO);
        FileUtil.DeleteSaveByName(levelName);
    }

    void LoadEmptyEditLevel(string levelName)
    {
        Destroy(createLevelCanvas.gameObject);

        ShowLevelToolCanvas();
        lvE.SetBlockCanvasShown(true);
        lvE.CreateNewLevel(levelName);
    }

    void ShowLevelToolCanvas()
    {
        levelToolCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/LevelEditorToolsCanvas")).GetComponent<Canvas>();
        GameObject.Find("BuildButton").GetComponent<Button>().onClick.AddListener(delegate { ShowBlockSelect(); });
        GameObject.Find("MenuButton").GetComponent<Button>().onClick.AddListener(delegate { ShowMenuCanvas(); });
        GameObject.Find("ColourButton").GetComponent<Button>().onClick.AddListener(delegate { ShowColourMenuCanvas(); });
        GameObject.Find("DemolishButton").GetComponent<Button>().onClick.AddListener(delegate { ShowDemolishCanvas(); });
    }

    void ShowDemolishCanvas()
    {
        if (showBlockCanvas != null) { Destroy(showBlockCanvas.gameObject); }
        if (ColourEditMenuCanvas != null) { Destroy(ColourEditMenuCanvas.gameObject); }
        lvE.demolishMode = true;
        demolishCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/DemolishMenuCanvas")).GetComponent<Canvas>();
        GameObject.Find("DMCloseButton").GetComponent<Button>().onClick.AddListener(delegate { HideDemolishCanvas(); });

    }

    void HideDemolishCanvas()
    {
        lvE.demolishMode = false;
        if (demolishCanvas != null) { Destroy(demolishCanvas.gameObject); }
    }

    void ShowMenuCanvas()
    {
        if (levelEditorMenuCanvas != null) { Destroy(levelEditorMenuCanvas.gameObject); }
        levelEditorMenuCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/LevelEditorMenuCanvas")).GetComponent<Canvas>();
        GameObject.Find("SaveButton").GetComponent<Button>().onClick.AddListener(delegate { lvE.SaveLevel(); });
        GameObject.Find("LoadButton").GetComponent<Button>().onClick.AddListener(delegate { DoStuff(); });
        GameObject.Find("LEMCloseButton").GetComponent<Button>().onClick.AddListener(delegate { HideMenuCanvas(); });
    }

    void ShowColourMenuCanvas()
    {
        if (showBlockCanvas != null) { Destroy(showBlockCanvas.gameObject); }
        if (ColourEditMenuCanvas != null) { Destroy(ColourEditMenuCanvas.gameObject); }
        if (demolishCanvas != null) { Destroy(demolishCanvas.gameObject); }
        ColourEditMenuCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/ColourEditMenuCanvas")).GetComponent<Canvas>();
        GameObject.Find("CMCloseButton").GetComponent<Button>().onClick.AddListener(delegate { HideColourCanvas(); });
        GameObject.Find("PaintModeDropdown").GetComponent<Dropdown>().onValueChanged.AddListener(delegate { ColourModeChanged(); });
        ColourModeChanged();
    }

    void ColourModeChanged()
    {
        GameObject dropdownGO = GameObject.Find("PaintModeDropdown");
        Dropdown dpd = dropdownGO.GetComponent<Dropdown>();
        if(dpd.value == 0)
        {
            //No Colour Mode
            Debug.Log("Switching to no colour paint mode");
            lvE.noColPaintMode = true;
            lvE.colourPaintMode = false;
        }else if(dpd.value == 1)
        {
            //Colour Mode
            lvE.noColPaintMode = false;
            lvE.colourPaintMode = true;
            Debug.Log("Switching to colour paint mode");
        }
        else
        {
            Debug.LogWarning("UNKOWN VALUE " + dpd.value);
        }
    }

    void HideColourCanvas()
    {
        if (ColourEditMenuCanvas != null) { Destroy(ColourEditMenuCanvas.gameObject); }
        lvE.noColPaintMode = false;
        lvE.colourPaintMode = false;
        lvE.demolishMode = false;
    }

    void DoStuff()
    {
        RemoveAllCanvases();
        LoadLevelButtonPressed();
    }

    void RemoveAllCanvases()
    {
        lvE.demolishMode = false;
        HideColourCanvas();
        if (demolishCanvas != null) { Destroy(demolishCanvas.gameObject); }
        if (showBlockCanvas != null) { Destroy(showBlockCanvas.gameObject); }
        if (levelToolCanvas != null) { Destroy(levelToolCanvas.gameObject); }
        if (levelEditorMenuCanvas != null) { Destroy(levelEditorMenuCanvas.gameObject); }
    }

    void HideMenuCanvas()
    {
        Destroy(levelEditorMenuCanvas.gameObject);
    }

    void ShowBlockSelect()
    {
        if (blockSelectShown == false)
        {
            blockSelectShown = true;
            Debug.Log("Generating Show Block UI....");
            if (ColourEditMenuCanvas != null) { Destroy(ColourEditMenuCanvas.gameObject); }
            if (demolishCanvas != null) { Destroy(demolishCanvas.gameObject); }
            showBlockCanvas = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/SelectBlockCanvas")).GetComponent<Canvas>();
            gameBlocks = GameObject.FindObjectOfType<GameBlocks>().GetComponent<GameBlocks>().gameBlocks;
            if (maximiseButtonCanvas != null) { Destroy(maximiseButtonCanvas.gameObject); }
            buttonPanel = GameObject.Find("SelectPanel");
            blockButtons.Clear();
            foreach (Block _b in gameBlocks)
            {
                Debug.Log("Adding " + _b.name + " To Selection UI...");
                GameObject buttonGO = GameObject.Instantiate((GameObject)Resources.Load("UI/LevelEditor/Block_Button"));
                buttonGO.transform.SetParent(buttonPanel.GetComponentInChildren<GridLayoutGroup>().gameObject.transform);
                buttonGO.name = _b.name;
                buttonGO.GetComponentInChildren<Text>().text = buttonGO.name;
                buttonGO.GetComponent<Button>().onClick.AddListener(delegate { SelectedBlockToPlace(gameBlocks.IndexOf(_b)); });
                GameObject.Find("CloseButton").GetComponent<Button>().onClick.AddListener(delegate { HideBlockSelect(); });
                _b.button = buttonGO.GetComponent<Button>();
                blockButtons.Add(_b.button);
                RawImage rI = buttonGO.GetComponentInChildren<RawImage>();
                Texture2D t2d = _b.icon;
                rI.texture = t2d;
                Debug.Log("Added " + _b.name + " To Selection UI");

            }
            SwitchButtonAllColors();
            LevelEditor lvE = GameObject.FindObjectOfType<LevelEditor>();
            lvE.SetBlockCanvasShown(true);
            Debug.Log("Finished Show Block UI");
        }
    }

    void HideBlockSelect()
    {
        if (blockSelectShown == true)
        {
            lvE.blockSelected = false;
            blockSelectShown = false;
            if (showBlockCanvas != null)
            {
                Destroy(showBlockCanvas.gameObject);
            }
            lvE.SetBlockCanvasShown(false);
        }
    }

    void GenStartGameUI()
    {

    }


	void SelectedBlockToPlace (int indexLocation) {
        Debug.Log("Selected " + gameBlocks[indexLocation].name + " In Level Editor");
        blockToPlace = gameBlocks[indexLocation];
        LevelEditor lvE = GameObject.FindObjectOfType<LevelEditor>();
        lvE.BlockSelectedChanged(blockToPlace);
        Block selectedBlock = gameBlocks[indexLocation];
        SwitchButtonColorsToBlock(selectedBlock);
        
	}

    void SwitchButtonColorsToBlock(Block _b)
    {
        foreach (Button b in blockButtons)
        {
            
            if (b != _b.button)
            {
                b.GetComponentInChildren<Text>().color = Color.black;
            }
            else
            {
                b.GetComponentInChildren<Text>().color = Color.green;
            }
        }
    }
    public void SwitchButtonAllColors()
    {
        for (int i = 0; i < blockButtons.Count; i++)
        {
            if (blockButtons[i] != null)
            {
                blockButtons[i].GetComponentInChildren<Text>().color = Color.black;
            }
            else
            {
                blockButtons.Remove(blockButtons[i]);
            }
        }
    }

}
