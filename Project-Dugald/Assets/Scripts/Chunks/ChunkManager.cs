using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
    public GameObject chunk;
    int chunkWidth;
    public int numChunks = 5;
	public int BiomeSize = 3;
    float seed;
    int sameBiome = 0;
    public TilePrefabs tilePref;
    public Tile SpawnPoint;
    private TileTypes.BiomeType previousBiomeType;
    [SerializeField]
    private TileTypes.BiomeType startBiomeType;

    public List<Tile> globalTileList = new List<Tile>();
    public List<GameObject> chunks = new List<GameObject>();
    public UIManager uiManager;
    public GameObject playerPrefab;
    public GameObject player = null;

    private int caveGenChunks;
    private int tileGenChunks;

    private int veinGenChunks;
    private int chunkGenChunks;
    private int treeGenChunks;
    private int flowerGenChunks;
	private int lakeGenChunks;

    // Use this for initialization
    void Start() {
        uiManager = GetComponent<UIManager>();
        tileGenChunks = numChunks;

        caveGenChunks = numChunks;
        veinGenChunks = numChunks;
        chunkGenChunks = numChunks;
        treeGenChunks = numChunks;
        flowerGenChunks = numChunks;
		lakeGenChunks = numChunks;

        tilePref = GetComponent<TilePrefabs>();
        chunkWidth = chunk.GetComponent<GenerateChunk>().width;
        seed = Random.Range(-10000, 10000);

    }

    // Update is called once per frame
    public void Generate() {
        int lastX = -chunkWidth;
        for (int chunkID = 0; chunkID < numChunks; chunkID++)
        {
            GameObject newChunk = Instantiate(chunk, new Vector3(lastX + chunkWidth, 0f), Quaternion.identity) as GameObject;
            newChunk.GetComponent<GenerateChunk>().chunkID = chunkID;
            newChunk.GetComponent<GenerateChunk>().tilePrefabs = tilePref;
            //Set Biome Type
            SetBiome(newChunk.GetComponent<GenerateChunk>());

            newChunk.GetComponent<GenerateChunk>().seed = seed;
            lastX += chunkWidth;
            newChunk.transform.SetParent(this.transform);
            newChunk.name = "Chunk: ID:" + chunkID;
            uiManager.SetText(0, "Loading: Generating Tiles For Chunk: " + chunkID + " With Biome Type: " + newChunk.GetComponent<GenerateChunk>().biomeType.ToString());
            newChunk.GetComponent<GenerateChunk>().chunkManager = this;
            chunks.Add(newChunk);
            
        }


        //StartCoroutine(genWorld());

    }

    #region Biome
    void SetBiome(GenerateChunk _newChunk)
    {
		
		int startNewBiome = Random.Range(0, BiomeSize);
        int nextBiomeType = Random.Range(1, 100);

        if (_newChunk.chunkID == 0)
        {
            //First Chunk
            previousBiomeType = startBiomeType;
            _newChunk.biomeType = startBiomeType;

        } else
        {
            
                if (startNewBiome >= sameBiome)
                {
                    sameBiome = sameBiome + 1;
                    //Continue Old Biome
                    _newChunk.biomeType = previousBiomeType;
                }
                else 
                {
                    //Start New Biome
                    sameBiome = 0;
				if (nextBiomeType > 1 && nextBiomeType < 10) {
					_newChunk.biomeType = TileTypes.BiomeType.Desert;
					previousBiomeType = TileTypes.BiomeType.Desert;
				} else if (nextBiomeType > 9 && nextBiomeType < 20) {
					_newChunk.biomeType = TileTypes.BiomeType.Ocean;
					previousBiomeType = TileTypes.BiomeType.Ocean;
				} else if (nextBiomeType > 19 && nextBiomeType < 30) {
					_newChunk.biomeType = TileTypes.BiomeType.Snow;
					previousBiomeType = TileTypes.BiomeType.Snow;
				} else if (nextBiomeType > 29 && nextBiomeType < 40) {
					_newChunk.biomeType = TileTypes.BiomeType.Mountain;
					previousBiomeType = TileTypes.BiomeType.Mountain;
				}else if (nextBiomeType > 39 && nextBiomeType < 50) {
					_newChunk.biomeType = TileTypes.BiomeType.Tundra;
					previousBiomeType = TileTypes.BiomeType.Tundra;
					} else if (nextBiomeType > 49 && nextBiomeType < 80) {
						_newChunk.biomeType = TileTypes.BiomeType.Plains;
						previousBiomeType = TileTypes.BiomeType.Plains;
					} else if (nextBiomeType > 79 && nextBiomeType < 101) {
						_newChunk.biomeType = TileTypes.BiomeType.Forest;
						previousBiomeType = TileTypes.BiomeType.Forest;
					}
                }
           
            }

        }


    
    #endregion

    public void SetupGlobalId(Tile _t)
    {
        globalTileList.Add(_t);
        if(_t.chunkID == 0)
        {
            _t.GlobalTileX = _t.x;
            _t.GlobalTileY = _t.y;
        }
        else
        {
            _t.GlobalTileX = _t.x + (chunkWidth * _t.chunkID);
            _t.GlobalTileY = _t.y;
        }

    }

	void setTileSides(){
        foreach (GameObject _chunk in chunks)
        {
            uiManager.SetText(0, "Loading: Setting Tile Sides For Chunk: " + _chunk.GetComponent<GenerateChunk>().chunkID);
            _chunk.GetComponent<GenerateChunk>().setTileSides();
        }
    }



    void genOres()
    {
        uiManager.SetText(0, "Loading: Generating Ores");
        chunks[0].GetComponent<GenerateChunk>().GenOres();
    }

    void genCaves()
    {

        foreach (GameObject _chunk in chunks)
        {
            uiManager.SetText(0, "Loading: Generating Caves For Chunk: " + _chunk.GetComponent<GenerateChunk>().chunkID);
            _chunk.GetComponent<GenerateChunk>().GenerateCaves();
        }
    }

    void genVeins()
    {

        foreach (GameObject _chunk in chunks)
        {
            uiManager.SetText(0, "Loading: Generating Veins For Chunk: " + _chunk.GetComponent<GenerateChunk>().chunkID);
            _chunk.GetComponent<GenerateChunk>().GenerateOreVeins();
        }
    }

    void genTrees()
    {

        foreach (GameObject _chunk in chunks)
        {
            uiManager.SetText(0, "Loading: Spawning Trees For Chunk: " + _chunk.GetComponent<GenerateChunk>().chunkID);
            _chunk.GetComponent<GenerateChunk>().GenerateTrees();
        }
    }
	void genFoliage()
	{
		foreach (GameObject _chunk in chunks) {
            uiManager.SetText(0, "Loading: Prettifying Chunk: " + _chunk.GetComponent<GenerateChunk>().chunkID + " With Flowers & Grass");
            _chunk.GetComponent<GenerateChunk>().GenerateFoliage();
		}
	}
	void genLakes()
	{
		foreach (GameObject _chunk in chunks) {
            uiManager.SetText(0, "Loading: Prettifying Chunk: " + _chunk.GetComponent<GenerateChunk>().chunkID + " With Lakes & Clouds");
            _chunk.GetComponent<GenerateChunk> ().GenerateLakes ();
		}
	}

    void genSpawnpoint()
    {
        uiManager.SetText(0, "Loading: Spawning Player");
        uiManager.SetText(0, "Loading: Done");
        uiManager.DisableAllCanvases();
        uiManager.EnableCanvas(0);
        //Multiple Spawn Points Later On??
        int chunkToPick = Mathf.RoundToInt(chunks.Count / 2);
        Debug.Log("Chunk Picked: " + chunkToPick);
        chunks[chunkToPick].GetComponent<GenerateChunk>().GenSpawnPoint();
    }

    public void finishOreGen()
    {

            Debug.Log("Finished Ore Gen");
            setTileSides();
        
    }

    public void finishTileSideGen()
    {
        
        tileGenChunks = tileGenChunks - 1;

        if (tileGenChunks == 0)
        {
            Debug.Log("Finished Tile Side Gen");
            genCaves();
        }
    }


    public void finishCaveGen()
    {

        caveGenChunks = caveGenChunks - 1;

        if (caveGenChunks == 0)
        {
            Debug.Log("Finished Generating Caves");
            genVeins();
        }
    }

    public void finishVeinGen()
    {

        veinGenChunks = veinGenChunks - 1;

        if (veinGenChunks == 0)
        {
            Debug.Log("Finished Generating Veins");
            genLakes();
        }
    }
	public void finishLakeGen()
	{
		lakeGenChunks = lakeGenChunks - 1;
		if (lakeGenChunks == 0) {
			Debug.Log ("Finished Generating Lakes");
			genTrees(); 
		}
	}

    public void finishTreeGen()
    {

        treeGenChunks = treeGenChunks - 1;

        if (treeGenChunks == 0)
        {
            Debug.Log("Finished Generating Trees");
            genFoliage();
        }
    }

    public void finishFlowerGen()
    {

        flowerGenChunks = flowerGenChunks - 1;

        if (flowerGenChunks == 0)
        {
            Debug.Log("Finished Generating Flowers");
            genSpawnpoint();
        }
    }

    public void finishChunkGen()
    {

        chunkGenChunks = chunkGenChunks - 1;

        if (chunkGenChunks == 0)
        {
            Debug.Log("Finished Generating Chunks");
            StartCoroutine(wait1());
        }
    }

    public void finishSpawnPointGen()
    {
        Debug.Log("Generated Spawn Point Successfully!");
        GameObject _player = Instantiate(playerPrefab, new Vector3(SpawnPoint.gameObject.transform.position.x, SpawnPoint.gameObject.transform.position.y + 2, -10), Quaternion.identity);
        player = _player;
        Camera.main.GetComponent<CameraFollow>().target = _player;
    }
    //Order Generated:
    //Gen Chunks
    //Gen Ores
    //Set Tile Sides
    //Gen Caves
    //Gen Veins
    //Gen Clouds
    #region Ienumerators
    IEnumerator wait1()
    {

        yield return new WaitForSeconds(1);
        genOres();
    }
    #endregion
}
