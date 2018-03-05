using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField]
    private GameObject parentObjectForTiles;

    [SerializeField]
    private float tileSize = 2f;

    private float xOffset = 2f;

    private float zOffset = 2f;

    private Tile[] mapTiles;
    [SerializeField]
    private GameObject[] tileTypes;
    //0-Land(NO TREES)
    //1-LAND(TREES)

    [SerializeField]
    private int mapWidth;
    [SerializeField]
    private int mapHeight;

	void Start () {

        xOffset = tileSize;
        zOffset = tileSize;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                int randomNum = Random.Range(0, 10);

                if (randomNum >= 5)
                {

                    GameObject _currentTile = (GameObject)Instantiate(tileTypes[1], new Vector3(x * xOffset, 1, z * zOffset), Quaternion.identity);
                    _currentTile.AddComponent<Tile>();
                    _currentTile.name = "Tile(" + x + "," + z + ")";
                    Tile _currentTileScript = _currentTile.GetComponent<Tile>();
                    _currentTileScript.tileType = Tile.TileType.LAND;
                    _currentTile.transform.SetParent(parentObjectForTiles.transform);
                    _currentTileScript.hasTrees = true;
                }
                else
                {

                    GameObject _currentTile = (GameObject)Instantiate(tileTypes[0], new Vector3(x * xOffset, 1, z * zOffset), Quaternion.identity);
                    _currentTile.AddComponent<Tile>();
                    _currentTile.name = "Tile(" + x + "," + z + ")";
                    Tile _currentTileScript = _currentTile.GetComponent<Tile>();
                    _currentTileScript.tileType = Tile.TileType.LAND;
                    _currentTile.transform.SetParent(parentObjectForTiles.transform);
                    _currentTileScript.hasTrees = false;
                }


            }
        }
	}
	

	void Update () {
		
	}
}
