using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTile : MonoBehaviour
{
    public int gridSize = 1;
    [HideInInspector]
    public Hotbar hotbar;
    [HideInInspector]
    public ItemList.ItemType selectedItem;
    [HideInInspector]
    public Item selectedItemScript;
    [HideInInspector]
    public GameObject gameManagerGO;
    [HideInInspector]
    public GameObject tempPlaceHolder;
    public float maxBreakDistance = 10;
    [HideInInspector]
    public GameObject lastTile;

    // Use this for initialization
    void Start()
    {
        gameManagerGO = GameObject.FindGameObjectWithTag("GM");
        hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();
    }

    void Update()
    {
        if (tempPlaceHolder != null) { Destroy(tempPlaceHolder); }
        selectedItem = hotbar.selectedItem;
        selectedItemScript = hotbar.selectedItemScript;
        if (selectedItemScript == null) { return; }
        if (gameManagerGO.GetComponent<CheckItems>().CheckIfItemIsBlock(selectedItem) == true)
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 snappedV3 = new Vector3(Mathf.Round(pz.x / gridSize), Mathf.Round(pz.y / gridSize), pz.z) * gridSize;
            snappedV3.z = 20;
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, snappedV3) < maxBreakDistance)
            {
                
                tempPlaceHolder = Instantiate(gameManagerGO.GetComponent<CheckItems>().ReturnItemTilePrefab(gameManagerGO.GetComponent<TilePrefabs>(), selectedItem), snappedV3, Quaternion.identity);
                tempPlaceHolder.GetComponent<BoxCollider2D>().isTrigger = true;

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Debug.Log("Mouse1");
                    RaycastHit2D hit4;
                    Vector3 rayOrigin4 = tempPlaceHolder.gameObject.transform.position;
                    hit4 = Physics2D.Raycast(rayOrigin4, Vector3.down, 1);

                    if (hit4.collider == null || hit4.collider.tag == "Player")
                    {
                        Debug.Log("a");
                        return;
                    }
                    else
                    {
                        if (hit4.collider.transform.gameObject.GetComponent<Tile>().isupTileNull == true && hit4.collider.isTrigger == false)
                        {
                            Debug.Log("l");
                            lastTile = gameManagerGO.GetComponent<ChunkManager>().chunks[hit4.collider.transform.gameObject.GetComponent<Tile>().genChunkScript.chunkID].GetComponent<GenerateChunk>().SpawnTile2(hit4.collider.transform.gameObject.GetComponent<Tile>().x - 1, hit4.collider.transform.gameObject.GetComponent<Tile>().y, gameManagerGO.GetComponent<CheckItems>().ReturnItemTilePrefab(gameManagerGO.GetComponent<TilePrefabs>(), selectedItem));
                            lastTile.GetComponent<Tile>().manMade = true;
                            Destroy(tempPlaceHolder);
                            return;
                        }
                        else
                        {
                            Debug.Log("b");
                            return;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        else { if (tempPlaceHolder != null) { Destroy(tempPlaceHolder); } }
    }
}
