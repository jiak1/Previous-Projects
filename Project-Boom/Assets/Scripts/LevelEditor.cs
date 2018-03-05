using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
public class LevelEditor : MonoBehaviour
{



    Block selectedBlock;
    [HideInInspector]
    public bool blockSelected = false;
    public bool inEditor = false;
    public bool colourPaintMode = false;
    public bool noColPaintMode = false;
    public bool demolishMode = false;
    float currentRotation = 0f;
    float gridSize = 1f;
    float rotateAmount = 90f;
    GameObject pastInst;
    bool blockCanvasShown = false;
    string levelName;
    float minToCamera = 2.0f;
    public GameObject lastSelectedPreview = null;
    public List<GameObject> mapData = new List<GameObject>();
    List<GameObject> previewBlocks = new List<GameObject>();
    Vector3 initialMousePosition;
    EventSystem ev;

    private void Start()
    {
        ev = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void BlockSelectedChanged(Block changedTo)
    {
        blockSelected = true;
        selectedBlock = changedTo;
        currentRotation = 0f;
    }

    private void Update()
    {
        DoBlockPlacer();


        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject zombieGO = GameObject.Instantiate((GameObject)Resources.Load("Enemy/ENEMY_ZOMBIE"));
            NavMeshAgent NVA = zombieGO.AddComponent<NavMeshAgent>();
            NVA.radius = 7.46f;
            NVA.height = 16f;
            zombieGO.AddComponent<EnemyZombieAI>();
        }
        if (noColPaintMode)
        {
            //No colour paint mode
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                if (Vector3.Distance(_hit.point, Camera.main.transform.position) > minToCamera)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        //Colour object
                        if (_hit.collider.gameObject != null)
                        {
                            Transform mainObj = Util.getInitialParent(_hit.collider.transform, this.transform.name);
                            Debug.Log("Painting " + mainObj.name + " To have no colour");
                            GOData god = mainObj.GetComponent<GOData>();
                            if (god != null) { god.coloured = false; }
                            //Add block to non-coloured layer
                            Util.MoveToLayer(mainObj, 10);
                        }
                    }
                    else
                    {
                        //Preview Selection
                        if (_hit.collider.gameObject != null)
                        {
                            Transform mainObj = Util.getInitialParent(_hit.collider.transform, this.transform.name);
                        }
                    }
                }
            }
        }
        else if (colourPaintMode)
        {
            //Colour paint mode
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                if (Vector3.Distance(_hit.point, Camera.main.transform.position) > minToCamera)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        //Colour Object
                        if (_hit.collider.gameObject != null)
                        {
                            Transform mainObj = Util.getInitialParent(_hit.collider.transform, this.transform.name);
                            Debug.Log("Painting " + mainObj.name + " To have colour");
                            GOData god = mainObj.GetComponent<GOData>();
                            if (god != null) { god.coloured = true; }
                            //Add block to coloured layer
                            Util.MoveToLayer(mainObj, 9);
                        }
                    }
                    else
                    {
                        //Preview Selection
                        if (_hit.collider.gameObject != null)
                        {
                            Transform mainObj = Util.getInitialParent(_hit.collider.transform, this.transform.name);
                        }
                    }
                }
            }
        }
        else if (demolishMode)
        {
            //Demolish mode
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                if (Vector3.Distance(_hit.point, Camera.main.transform.position) > minToCamera)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        //Delete object
                        if (_hit.collider.gameObject != null)
                        {
                            Transform mainObj = Util.getInitialParent(_hit.collider.transform, this.transform.name);
                            Debug.Log("Deleting: " + mainObj.name);
                            if (mapData.Contains(mainObj.gameObject)) { mapData.Remove(mainObj.gameObject); Destroy(mainObj.gameObject); }
                        }
                    }
                    else
                    {
                        //Preview Selection
                        if (_hit.collider.gameObject != null)
                        {
                            
                            
                            Transform mainObj = Util.getInitialParent(_hit.collider.transform, this.transform.name);
                            MeshRenderer[] mr = mainObj.GetComponentsInChildren<MeshRenderer>();
                            lastSelectedPreview = mainObj.gameObject;
                            foreach (MeshRenderer _mr in mr)
                            {
                                ShowPreview sp = _mr.gameObject.AddComponent<ShowPreview>();
                                sp.Load(mainObj.gameObject, this,_mr);
                            }
                            
                        }
                    }
                }
            }
        }

    }


    public void SetBlockCanvasShown(bool shown)
    {
        blockCanvasShown = shown;
    }

    public void SaveLevel()
    {
        FileUtil.SaveGame(levelName, mapData);
    }

    void DoBlockPlacer()
    {
        if (selectedBlock == null) { blockSelected = false; }
        if (pastInst != null) { Destroy(pastInst); }
        if (blockCanvasShown)
        {
            if (Input.GetKeyDown(KeyCode.Q) && blockSelected)
            {
                currentRotation += rotateAmount;
            }
            if (Input.GetKeyDown(KeyCode.E) && blockSelected)
            {
                currentRotation -= rotateAmount;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                blockSelected = false;
                selectedBlock = null;
                UIManager uiM = GameObject.FindObjectOfType<UIManager>();
                uiM.SwitchButtonAllColors();
            }
            else if (blockSelected == true && selectedBlock != null && ev.IsPointerOverGameObject() == false)
            {
                //When the mouse is pressed get the initial position of it
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    RaycastHit _hit;
                    Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                    {
                        if (Vector3.Distance(_hit.point, Camera.main.transform.position) > minToCamera)
                        {
                            //Mouse pressed get inital position to determine whether selection or press occured
                            initialMousePosition = _hit.point;
                        }
                    }
                }
                else
                {
                    //When the mouse is released check if selection or press occured then instantiate
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        //Mouse released instantiate blocks
                        RaycastHit _hit;
                        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Vector3 mousePos = initialMousePosition;
                        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                        {
                            if (Vector3.Distance(_hit.point, Camera.main.transform.position) > minToCamera)
                            {
                                mousePos = _hit.point;
                            }
                        }
                        if (mousePos != initialMousePosition)
                        {
                            //Debug.Log("selection placing");
                            //Selection Occured Place selectecd blocks in area
                            PlaceSelectionOfBlocks(initialMousePosition, mousePos);
                        }
                        else
                        {
                            //Press occured place single block
                            //Debug.Log("placing");
                            PlaceBlockAtMousePos();
                        }
                        //Mouse still pressed preview block or selection
                    }
                    else if (Input.GetKey(KeyCode.Mouse0))
                    {
                        //Mouse held down
                        RaycastHit _hit;
                        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Vector3 mousePos = initialMousePosition;
                        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
                        {
                            if (Vector3.Distance(_hit.point, Camera.main.transform.position) > minToCamera)
                            {
                                mousePos = _hit.point;
                            }
                        }
                        if (mousePos != initialMousePosition)
                        {
                            //Selection Occured Preview
                            PreviewSelectionOfBlocks(initialMousePosition, mousePos);
                        }
                        else
                        {
                            //Press occured preview
                            PreviewBlockAtMousePos();
                        }

                    }
                    else
                    {
                        //No key pressed but block still selected so preview
                        PreviewBlockAtMousePos();
                    }
                }

                //IF RAYCAST IS NOT WORKING CHECK IT IS HITTING A COLLIDER
            }
        }
    }

    GameObject CreateBlock(GameObject _prefab, Vector3 _position, float _yRotation)
    {
        GameObject newInst = Instantiate(_prefab);
        newInst.transform.position = _position;
        newInst.transform.RotateAround(newInst.transform.position, Vector3.up, _yRotation);
        newInst.transform.SetParent(transform, false);
        GOData goD = newInst.AddComponent<GOData>();
        goD.blockTypeName = _prefab.name;
        goD.position = newInst.transform.position;
        goD.yRotation = _yRotation;
        newInst.name = _prefab.name + ", X:" + newInst.transform.position.x + ", Y:" + newInst.transform.position.y + ", Z:" + newInst.transform.position.z;
        newInst.GetComponentInChildren<MeshRenderer>().gameObject.AddComponent<NavMeshSourceTag>();
        Util.AddAllMeshCollidersToRenderers(newInst);
        mapData.Add(newInst);
        return newInst;
    }

    public void LoadOldLevel(string _levelName)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform _t in this.transform)
        {
            if (_t.name != "LevelBottom")
            {
                children.Add(_t);
            }
        }
        for (int i = 0; i < children.Count; i++)
        {
            Destroy(children[i].gameObject);
        }
        MapData mapData = FileUtil.loadSave(_levelName);
        GameBlocks gb = GameObject.FindObjectOfType<GameBlocks>().GetComponent<GameBlocks>();
        if (mapData == null) { Debug.LogError("ERRORR"); }
        foreach (BlockEntry be in mapData.mapBlocks)
        {
            if (gb.gameBlockStrings.Contains(be.blockName))
            {
                Block block = gb.gameBlocks[gb.gameBlockStrings.IndexOf(be.blockName)];
                GameObject blockGO = CreateBlock(block.prefab, be.blockPosition, be.blockYRotation);
                if (be.coloured)
                {
                    //Add block to coloured layer
                    Util.MoveToLayer(blockGO.transform, 9);
                }
                else
                {
                    //Add block to non-coloured layer
                    Util.MoveToLayer(blockGO.transform, 10);
                }
            }
            else
            {
                Debug.LogWarning("WARNING: OBJECT WITH NAME " + be.blockName + " NO LONGER EXISTS FOR SAVE FILE IT MAY HAVE BEEN REMOVED FROM GAME OR PART OF A MOD!!");
            }
        }
        levelName = _levelName;
        inEditor = true;
    }

    public void CreateNewLevel(string _levelName)
    {
        levelName = _levelName;
        Debug.Log("Created Empty Level With Name: " + _levelName);
        inEditor = true;
    }


    void PlaceBlockAtMousePos()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (Vector3.Distance(hit.point, Camera.main.transform.position) > minToCamera)
            {
                GameObject newInst = Instantiate(selectedBlock.prefab);
                //newInst.transform.position = new Vector3(Mathf.Round(hit.point.x / gridSize) * gridSize, Mathf.Round(hit.point.y / gridSize) * gridSize, Mathf.Round(hit.point.z / gridSize) * gridSize);
                newInst.transform.position = new Vector3(Mathf.Round(hit.point.x),
                     Mathf.Round(hit.point.y),
                     Mathf.Round(hit.point.z));
                newInst.transform.RotateAround(newInst.transform.position, Vector3.up, currentRotation);
                Util.DisableAllMeshCollidersInChildren(newInst);
                pastInst = newInst;
                Vector3 position = new Vector3(Mathf.Round(hit.point.x / gridSize) * gridSize, Mathf.Round(hit.point.y / gridSize) * gridSize, Mathf.Round(hit.point.z / gridSize) * gridSize);
                CreateBlock(selectedBlock.prefab, position, currentRotation);
            }

        }
    }


    void PreviewBlockAtMousePos()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (Vector3.Distance(hit.point, Camera.main.transform.position) > minToCamera)
            {
                GameObject newInst = Instantiate(selectedBlock.prefab);
                //newInst.transform.position = new Vector3(Mathf.Round(hit.point.x / gridSize) * gridSize, Mathf.Round(hit.point.y / gridSize) * gridSize, Mathf.Round(hit.point.z / gridSize) * gridSize);
                newInst.transform.position = new Vector3(Mathf.Round(hit.point.x),
                     Mathf.Round(hit.point.y),
                     Mathf.Round(hit.point.z));
                newInst.transform.RotateAround(newInst.transform.position, Vector3.up, currentRotation);
                newInst.transform.name = "PreviewBlock";
                Util.DisableAllMeshCollidersInChildren(newInst);
                pastInst = newInst;

            }

        }
    }

    void PlaceSelectionOfBlocks(Vector3 _initialPos, Vector3 _endPos)
    {
        RemovePreviousPreviewBlocks();
        float blockLength = selectedBlock.prefab.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        float blockWidth = selectedBlock.prefab.GetComponentInChildren<MeshRenderer>().bounds.size.z;

        for (float x = Mathf.Min(_initialPos.x, _endPos.x); x < Mathf.Max(_initialPos.x, _endPos.x) - Mathf.Epsilon; x += blockLength)
        {
            for (float z = Mathf.Min(_initialPos.z, _endPos.z); z < Mathf.Max(_initialPos.z, _endPos.z) - Mathf.Epsilon; z += blockWidth)
            {
                GameObject inst = CreateBlock(selectedBlock.prefab, new Vector3(x, _initialPos.y, z), currentRotation);
            }
        }
    }

    void RemovePreviousPreviewBlocks()
    {
        for (int i = 0; i < previewBlocks.Count; i++)
        {
            Destroy(previewBlocks[i]);
        }
        previewBlocks.Clear();
    }
    void PreviewSelectionOfBlocks(Vector3 _initialPos, Vector3 _endPos)
    {
        RemovePreviousPreviewBlocks();
        float blockLength = selectedBlock.prefab.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        float blockWidth = selectedBlock.prefab.GetComponentInChildren<MeshRenderer>().bounds.size.z;

        for (float x = Mathf.Min(_initialPos.x, _endPos.x); x < Mathf.Max(_initialPos.x, _endPos.x) - Mathf.Epsilon; x += blockLength)
        {
            for (float z = Mathf.Min(_initialPos.z, _endPos.z); z < Mathf.Max(_initialPos.z, _endPos.z) - Mathf.Epsilon; z += blockWidth)
            {
                GameObject inst = CreateBlock(selectedBlock.prefab, new Vector3(x, _initialPos.y, z), currentRotation);
                Util.DisableAllMeshCollidersInChildren(inst);
                previewBlocks.Add(inst);
            }
        }

    }
}
