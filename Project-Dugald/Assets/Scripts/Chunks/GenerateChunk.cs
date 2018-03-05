using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChunk : MonoBehaviour
{


    public TilePrefabs tilePrefabs;
    public ChunkManager chunkManager;

    public int buildHeight;
    public int width;
    public float heightMultiplier;
    public int heightAddition;
    public float smoothnessDampner;

    [Header("Ores:")]
    [Header("Coal:")]
    public float chanceCoal;
    public int coalMinY;
    public int coalMaxY;
    [Header("Iron:")]
    public float chanceIron;
    public int ironMinY;
    public int ironMaxY;
    [Header("Gold:")]
    public float chanceGold;
    public int goldMinY;
    public int goldMaxY;
    [Header("Diamond:")]
    public float chanceDiamond;
    public int diamondMinY;
    public int diamondMaxY;

    [HideInInspector]
    public TileTypes.BiomeType biomeType;

    private TileTypes.TerrainType terrainType;
    private TileTypes.TerrainType oreTerrainType;

    public List<Tile> tiles = new List<Tile>();
    public List<Tile> surfaceTiles = new List<Tile>();

    public bool isSurface;
    public bool isEnabled = false;
    public int enableDistance = 100;
    [HideInInspector]
    public float seed;
    [HideInInspector]
    public int chunkID;
    private bool waiting = false;
    void Start()
    {
        // seed = Random.Range(-10000f, 10000f);
        Generate();
    }

    private void FixedUpdate()
    {
        if (chunkManager.player == null)
        {
            return;
        }
        else
        {
            if (Vector3.Distance(chunkManager.player.transform.position, this.transform.position) > enableDistance)
            {
                // Debug.Log("Distance Between Chunk" + chunkID + " and player is: " + Vector3.Distance(chunkManager.player.transform.position, this.transform.position));
                isEnabled = false;
            }
            else
            {
                isEnabled = true;
                if (waiting == false)
                {
                    waiting = true;
                }

            }
        }
    }

    public GameObject SpawnTile2(int x, int y, GameObject selectedTile)
    {

        GameObject instantiatedTile = Instantiate(selectedTile, new Vector3(x, y), Quaternion.identity);
        instantiatedTile.transform.SetParent(this.transform);
        instantiatedTile.name = "Tile: X:" + (x + 1) + ", Y:" + (y + 1);
        instantiatedTile.AddComponent<Tile>();
        Tile currentTile = instantiatedTile.GetComponent<Tile>();
        currentTile.chunkID = chunkID;

        currentTile.x = x + 1;
        currentTile.y = y + 1;
        currentTile.biomeType = biomeType;
        currentTile.terrainType = terrainType;
        chunkManager.SetupGlobalId(currentTile);
        tiles.Add(currentTile);

        List<TileTypes.TerrainType> ores = new List<TileTypes.TerrainType>();

        ores.Add(TileTypes.TerrainType.Coal);
        ores.Add(TileTypes.TerrainType.Diamond);
        ores.Add(TileTypes.TerrainType.Gold);
        ores.Add(TileTypes.TerrainType.Iron);
        ores.Add(TileTypes.TerrainType.Stone);

        currentTile.genChunkScript = this;
        if (ores.Contains(currentTile.terrainType) && !(chunkManager.chunks[currentTile.chunkID].GetComponent<GenerateChunk>().biomeType == TileTypes.BiomeType.Desert))
        {
            currentTile.backgroundMat = tilePrefabs.stoneBackgroundMat;
        }

        if (currentTile.terrainType == TileTypes.TerrainType.Dirt || currentTile.terrainType == TileTypes.TerrainType.Grass)
        {
            currentTile.backgroundMat = tilePrefabs.dirtBackgroundMat;
        }

        if (currentTile.terrainType == TileTypes.TerrainType.Sand || currentTile.terrainType == TileTypes.TerrainType.Sandstone || ores.Contains(currentTile.terrainType) && chunkManager.chunks[currentTile.chunkID].GetComponent<GenerateChunk>().biomeType == TileTypes.BiomeType.Desert)
        {
            currentTile.backgroundMat = tilePrefabs.sandBackgroundMat;
        }

        instantiatedTile.transform.localPosition = new Vector3(x, y);
        return instantiatedTile;
    }

    public void SpawnTile(int x, int y, GameObject selectedTile)
    {

        GameObject instantiatedTile = Instantiate(selectedTile, new Vector3(x, y), Quaternion.identity);
        instantiatedTile.transform.SetParent(this.transform);
        instantiatedTile.name = "Tile: X:" + (x + 1) + ", Y:" + (y + 1);
        instantiatedTile.AddComponent<Tile>();
        Tile currentTile = instantiatedTile.GetComponent<Tile>();
        currentTile.chunkID = chunkID;


        currentTile.x = x + 1;
        currentTile.y = y + 1;
        currentTile.biomeType = biomeType;
        currentTile.terrainType = terrainType;
        tiles.Add(currentTile);
        if (isSurface == true) { surfaceTiles.Add(currentTile); }

        chunkManager.SetupGlobalId(currentTile);
        currentTile.genChunkScript = this;
        List<TileTypes.TerrainType> ores = new List<TileTypes.TerrainType>();

        ores.Add(TileTypes.TerrainType.Coal);
        ores.Add(TileTypes.TerrainType.Diamond);
        ores.Add(TileTypes.TerrainType.Gold);
        ores.Add(TileTypes.TerrainType.Iron);
        ores.Add(TileTypes.TerrainType.Stone);

        currentTile.genChunkScript = this;
        if (ores.Contains(currentTile.terrainType) && !(chunkManager.chunks[currentTile.chunkID].GetComponent<GenerateChunk>().biomeType == TileTypes.BiomeType.Desert))
        {
            currentTile.backgroundMat = tilePrefabs.stoneBackgroundMat;
        }

        if (currentTile.terrainType == TileTypes.TerrainType.Dirt || currentTile.terrainType == TileTypes.TerrainType.Grass)
        {
            currentTile.backgroundMat = tilePrefabs.dirtBackgroundMat;
        }

        if (currentTile.terrainType == TileTypes.TerrainType.Sand || currentTile.terrainType == TileTypes.TerrainType.Sandstone || ores.Contains(currentTile.terrainType) && chunkManager.chunks[currentTile.chunkID].GetComponent<GenerateChunk>().biomeType == TileTypes.BiomeType.Desert)
        {
            currentTile.backgroundMat = tilePrefabs.sandBackgroundMat;
        }

        if (currentTile.terrainType == TileTypes.TerrainType.Dirt || currentTile.terrainType == TileTypes.TerrainType.Grass || currentTile.terrainType == TileTypes.TerrainType.Snow)
        {
            //TopTiles.Add(currentTile);
        }
        instantiatedTile.transform.localPosition = new Vector3(x, y);
    }

    public void Generate()
    {


        for (int i = 0; i < width; i++)
        {

            GameObject selectedTile;
            #region Plains
            //terrain generation for plains
            if (biomeType == TileTypes.BiomeType.Plains)
            {
                //generates the random number for procedural generation
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * (heightMultiplier / 2)) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed
                    if (j < h - 4)
                    { selectedTile = tilePrefabs.stoneTile; terrainType = TileTypes.TerrainType.Stone; }
                    else if (j < h - 1) // sets all tiles 1 block below the surface to dirt
                    { selectedTile = tilePrefabs.dirtTile; terrainType = TileTypes.TerrainType.Dirt; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.grassTile; terrainType = TileTypes.TerrainType.Grass; isSurface = true; }
                    else //sets surface tiles to grass
                    { selectedTile = tilePrefabs.grassTile; terrainType = TileTypes.TerrainType.Grass; }
                    //spawns in the tiles
                    SpawnTile(i, j, selectedTile);
                }
            }
            #endregion
            #region Forest
            else if (biomeType == TileTypes.BiomeType.Forest)
            {
                //generates the random number for procedural generation
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * heightMultiplier) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed
                    if (j < h - 4)
                    { selectedTile = tilePrefabs.stoneTile; terrainType = TileTypes.TerrainType.Stone; }
                    else if (j < h - 1) // sets all tiles 1 block below the surface to dirt
                    { selectedTile = tilePrefabs.dirtTile; terrainType = TileTypes.TerrainType.Dirt; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.grassTile; terrainType = TileTypes.TerrainType.Grass; isSurface = true; }
                    else //sets surface tiles to grass
                    { selectedTile = tilePrefabs.grassTile; terrainType = TileTypes.TerrainType.Grass; }
                    //spawns in the tiles
                    SpawnTile(i, j, selectedTile);
                }
                #endregion
                #region Desert
            } //terrain generation for deserts
            else if (biomeType == TileTypes.BiomeType.Desert)
            {	//generates the random number for procedural generation
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * heightMultiplier) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed
                    if (j < h - 4)  // sets all tiles 4 blocks below surface to stone
                    { selectedTile = tilePrefabs.sandstoneTile; terrainType = TileTypes.TerrainType.Sandstone; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.sandTile; terrainType = TileTypes.TerrainType.Sand; isSurface = true; }
                    else //sets all other tiles to sand
                    { selectedTile = tilePrefabs.sandTile; terrainType = TileTypes.TerrainType.Sand; }
                    //spawns in the tiles
                    SpawnTile(i, j, selectedTile);
                }
                #endregion
                #region Ocean
            } // terrain generation for oceans 
            else if (biomeType == TileTypes.BiomeType.Ocean)
            {	//generates the random number for procedural generation 
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * heightMultiplier) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed	
                    if (j < h - 10)//sets all tiles 10 below the surface to stone
                    { selectedTile = tilePrefabs.stoneTile; terrainType = TileTypes.TerrainType.Stone; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.waterTile; terrainType = TileTypes.TerrainType.Water; isSurface = true; }
                    else //sets all other tiles to water 
                    { selectedTile = tilePrefabs.waterTile; terrainType = TileTypes.TerrainType.Water; }
                    //spawns in the tiles
                    SpawnTile(i, j, selectedTile);
                }
                #endregion
                #region Mountain
            }// terrain generation for Mountains
            else if (biomeType == TileTypes.BiomeType.Mountain)
            {	//generates the random number for procedural generation
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * heightMultiplier * 4) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed
                    if (j < h - 4)// sets all tiles 4 below the surface to stone
                    { selectedTile = tilePrefabs.stoneTile; terrainType = TileTypes.TerrainType.Stone; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.dirtTile; terrainType = TileTypes.TerrainType.Dirt; isSurface = true; }
                    else //sets all other tiles to dirt
                    { selectedTile = tilePrefabs.dirtTile; terrainType = TileTypes.TerrainType.Dirt; }
                    //spawns in the tiles	
                    SpawnTile(i, j, selectedTile);
                }
                #endregion
                #region Snow
            }// terrain generation for Snow
            else if (biomeType == TileTypes.BiomeType.Snow)
            {	//generates the random number for procedural generation
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * heightMultiplier) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed
                    if (j < h - 4)//sets all tiles 4 below surface to stone
                    { selectedTile = tilePrefabs.stoneTile; terrainType = TileTypes.TerrainType.Stone; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.snowTile; terrainType = TileTypes.TerrainType.Snow; isSurface = true; }
                    else// sets all other tiles to grass
                    { selectedTile = tilePrefabs.snowTile; terrainType = TileTypes.TerrainType.Snow; }
                    //spawns in the tiles
                    SpawnTile(i, j, selectedTile);
                }
            }
            #endregion
            #region Tundra
            else if (biomeType == TileTypes.BiomeType.Tundra)
            { //terrain generation for Tundra
                int h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x) / smoothnessDampner) * heightMultiplier * 2) + heightAddition;
                // h = terrain height
                for (int j = 0; j < h; j++)
                {
                    //determines what kind of tile will be placed
                    if (j < h - 2)//sets all tiles 4 below surface to stone
                    { selectedTile = tilePrefabs.stoneTile; terrainType = TileTypes.TerrainType.Stone; }
                    else if (j < h)
                    { selectedTile = tilePrefabs.snowTile; terrainType = TileTypes.TerrainType.Snow; isSurface = true; }
                    else// sets all other tiles to snow
                    { selectedTile = tilePrefabs.snowTile; terrainType = TileTypes.TerrainType.Snow; }
                    //spawns in the tiles
                    SpawnTile(i, j, selectedTile);
                }
            }
        }
        #endregion
        //Debug.Log("Finished Chunk Gen On Chunk: " + chunkID);
        chunkManager.finishChunkGen();
    }

    #region GenOres
    public void GenOres()
    {
        if (tiles.Count == 0)
        {
            //Debug.Log("Tiles Empty");
        }
        else
        {

            // Debug.Log("Tiles Count: " + tiles.Count);
        }
        //Is this neccessary to be called once or on all chunks??
        //Debug.Log("Calling Gen Ores On Chunk " + chunkID);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("TileStone"))
        {

            //Debug.Log("Rnng");
            Tile oldTileData = t.GetComponent<Tile>();
            float r = Random.Range(0f, 100f);
            GameObject selectedTile = null;

            if (oldTileData.y > diamondMinY && oldTileData.y < diamondMaxY)
            {
                if (r < chanceDiamond)
                {
                    selectedTile = tilePrefabs.diamondTile;
                    oreTerrainType = TileTypes.TerrainType.Diamond;

                }
                else if (oldTileData.y > goldMinY && oldTileData.y < goldMaxY)
                {
                    if (r < chanceGold)
                    {

                        selectedTile = tilePrefabs.goldTile;
                        oreTerrainType = TileTypes.TerrainType.Gold;
                    }
                    else if (oldTileData.y > ironMinY && oldTileData.y < ironMaxY)
                    {
                        if (r < chanceIron)
                        {
                            selectedTile = tilePrefabs.ironTile;
                            oreTerrainType = TileTypes.TerrainType.Iron;
                        }
                        else if (oldTileData.y > coalMinY && oldTileData.y < coalMaxY)
                        {
                            if (r < chanceCoal)
                            {
                                selectedTile = tilePrefabs.coalTile;
                                oreTerrainType = TileTypes.TerrainType.Coal;
                            }
                        }
                    }
                }
                else if (oldTileData.y > ironMinY && oldTileData.y < ironMaxY)
                {
                    if (r < chanceIron)
                    {
                        selectedTile = tilePrefabs.ironTile;
                        oreTerrainType = TileTypes.TerrainType.Iron;
                    }
                    else if (oldTileData.y > coalMinY && oldTileData.y < coalMaxY)
                    {
                        if (r < chanceCoal)
                        {
                            selectedTile = tilePrefabs.coalTile;
                            oreTerrainType = TileTypes.TerrainType.Coal;
                        }
                    }
                }
                else if (oldTileData.y > coalMinY && oldTileData.y < coalMaxY)
                {
                    if (r < chanceCoal)
                    {
                        selectedTile = tilePrefabs.coalTile;
                        oreTerrainType = TileTypes.TerrainType.Coal;
                    }
                }
            }
            else if (oldTileData.y > goldMinY && oldTileData.y < goldMaxY)
            {
                if (r < chanceGold)
                {

                    selectedTile = tilePrefabs.goldTile;
                    oreTerrainType = TileTypes.TerrainType.Gold;
                }
                else if (oldTileData.y > ironMinY && oldTileData.y < ironMaxY)
                {
                    if (r < chanceIron)
                    {
                        selectedTile = tilePrefabs.ironTile;
                        oreTerrainType = TileTypes.TerrainType.Iron;
                    }
                    else if (oldTileData.y > coalMinY && oldTileData.y < coalMaxY)
                    {
                        if (r < chanceCoal)
                        {
                            selectedTile = tilePrefabs.coalTile;
                            oreTerrainType = TileTypes.TerrainType.Coal;
                        }
                    }
                }

            }
            else if (oldTileData.y > ironMinY && oldTileData.y < ironMaxY)
            {
                if (r < chanceIron)
                {
                    selectedTile = tilePrefabs.ironTile;
                    oreTerrainType = TileTypes.TerrainType.Iron;
                }
                else if (oldTileData.y > coalMinY && oldTileData.y < coalMaxY)
                {
                    if (r < chanceCoal)
                    {
                        selectedTile = tilePrefabs.coalTile;
                        oreTerrainType = TileTypes.TerrainType.Coal;
                    }
                }
            }
            else if (oldTileData.y > coalMinY && oldTileData.y < coalMaxY)
            {
                if (r < chanceCoal)
                {
                    selectedTile = tilePrefabs.coalTile;
                    oreTerrainType = TileTypes.TerrainType.Coal;
                }
            }



            if (selectedTile != null)
            {
                GameObject newOreTile = Instantiate(selectedTile, t.transform.position, Quaternion.identity) as GameObject;


                newOreTile.AddComponent<Tile>();
                Tile currentTile = newOreTile.GetComponent<Tile>();

                currentTile.x = oldTileData.x;
                currentTile.y = oldTileData.y;
                currentTile.chunkID = oldTileData.chunkID;
                currentTile.terrainType = oreTerrainType;
                currentTile.name = "Tile: X:" + oldTileData.x + ", Y:" + oldTileData.y;
                currentTile.backgroundMat = tilePrefabs.stoneBackgroundMat;

                currentTile.gameObject.transform.SetParent(chunkManager.chunks[currentTile.chunkID].gameObject.transform);
                currentTile.genChunkScript = chunkManager.chunks[currentTile.chunkID].GetComponent<GenerateChunk>();
                tiles.Add(currentTile);
                tiles.Remove(oldTileData);
                Destroy(t);

            }


        }


        // Debug.Log("Finished Gen Ores On Chunk " + chunkID);
        chunkManager.finishOreGen();


    }


    #endregion
    #region SettileSides
    public void setTileSides()
    {
        //Debug.Log("Running SetTileSides On Chunk " + chunkID);

        for (int i = 0; i < tiles.Count; i++)
        {
            Tile _tile1 = tiles[i];

            RaycastHit2D hit;
            Vector3 rayOrigin = _tile1.gameObject.transform.position;
            hit = Physics2D.Raycast(rayOrigin, Vector3.left, 1);

            if (hit.collider == null || hit.collider.tag == "Player")
            {
                _tile1.leftTile = _tile1;
                _tile1.leftTile.isleftTileNull = true;
            }
            else
            {

                if (hit.transform.gameObject.GetComponent<Tile>() != null)
                {
                    _tile1.leftTile = hit.collider.transform.gameObject.GetComponent<Tile>();
                }


            }
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            Tile _tile1 = tiles[i];

            RaycastHit2D hit;
            Vector3 rayOrigin = _tile1.gameObject.transform.position;
            hit = Physics2D.Raycast(rayOrigin, Vector3.right, 1);

            if (hit.collider == null || hit.collider.tag == "Player")
            {
                _tile1.rightTile = _tile1;
                _tile1.rightTile.isrightTileNull = true;
            }
            else
            {

                if (hit.transform.gameObject.GetComponent<Tile>() != null)
                {
                    _tile1.rightTile = hit.collider.transform.gameObject.GetComponent<Tile>();
                }


            }
        }


        for (int i = 0; i < tiles.Count; i++)
        {
            Tile _tile1 = tiles[i];

            RaycastHit2D hit;
            Vector3 rayOrigin = _tile1.gameObject.transform.position;
            hit = Physics2D.Raycast(rayOrigin, Vector3.up, 1);

            if (hit.collider == null || hit.collider.tag == "Player")
            {
                _tile1.upTile = _tile1;
                _tile1.upTile.isupTileNull = true;
            }
            else
            {

                if (hit.transform.gameObject.GetComponent<Tile>() != null)
                {
                    _tile1.upTile = hit.collider.transform.gameObject.GetComponent<Tile>();
                }


            }
        }


        for (int i = 0; i < tiles.Count; i++)
        {
            Tile _tile1 = tiles[i];

            RaycastHit2D hit;
            Vector3 rayOrigin = _tile1.gameObject.transform.position;
            hit = Physics2D.Raycast(rayOrigin, Vector3.down, 1);

            if (hit.collider == null || hit.collider.tag == "Player")
            {
                _tile1.downTile = _tile1;
                _tile1.downTile.isdownTileNull = true;
            }
            else
            {

                if (hit.transform.gameObject.GetComponent<Tile>() != null)
                {
                    _tile1.downTile = hit.collider.transform.gameObject.GetComponent<Tile>();
                }


            }
        }
		Debug.Log("Finished Running SetTileSides On Chunk " + chunkID);

        chunkManager.finishTileSideGen();
    }
    #endregion
    #region GenCaves
    public void GenerateCaves()
    {
        Debug.Log("Generating Caves For Chunk:" + chunkID);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("TileStone"))
        {
            if (t == null)
            {

            }
            int rnd = Random.Range(0, 1000);
            if (rnd == 1)
            {
                Tile _tile = t.GetComponent<Tile>();
                if (_tile.gameObject == null)
                {

                }
                else
                {
                    _tile.TurnIntoCave();
                }
            }
        }
        Debug.Log("Finished Cave Gen Chunk: " + chunkID);
        chunkManager.finishCaveGen();
    }
    #endregion
    #region GenVeins
    public void GenerateOreVeins()
    {
        Debug.Log("Generating Veins For Chunk:" + chunkID);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("TileOre"))
        {
            Tile tiledata = t.GetComponent<Tile>();
            #region Coal
            if (tiledata.terrainType == TileTypes.TerrainType.Coal)
            {
                TileTypes.TerrainType cType = TileTypes.TerrainType.Coal;
                Material cMat = tilePrefabs.coalMat;
                int rnd1 = Random.Range(0, 10);
                int rnd2 = Random.Range(0, 10);
                int rnd3 = Random.Range(0, 10);
                int rnd4 = Random.Range(0, 10);

                if (rnd1 >= 6)
                {
                    if (tiledata.isleftTileNull == false)
                    {
                        tiledata.leftTile.terrainType = cType;
                        tiledata.leftTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd2 >= 6)
                    {
                        if (tiledata.isrightTileNull == false)
                        {
                            tiledata.rightTile.terrainType = cType;
                            tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                        }
                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd2 >= 6)
                {
                    if (tiledata.isrightTileNull == false)
                    {
                        tiledata.rightTile.terrainType = cType;
                        tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd3 >= 6)
                {
                    if (tiledata.isupTileNull == false)
                    {
                        tiledata.upTile.terrainType = cType;
                        tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd4 >= 6)
                {
                    if (tiledata.isdownTileNull == false)
                    {
                        tiledata.downTile.terrainType = cType;
                        tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                }
                #endregion
                #region Iron
            }
            else if (tiledata.terrainType == TileTypes.TerrainType.Iron)
            {

                TileTypes.TerrainType cType = TileTypes.TerrainType.Iron;
                Material cMat = tilePrefabs.ironMat;
                int rnd1 = Random.Range(0, 10);
                int rnd2 = Random.Range(0, 10);
                int rnd3 = Random.Range(0, 10);
                int rnd4 = Random.Range(0, 10);

                if (rnd1 >= 6)
                {
                    if (tiledata.isleftTileNull == false)
                    {
                        tiledata.leftTile.terrainType = cType;
                        tiledata.leftTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd2 >= 6)
                    {
                        if (tiledata.isrightTileNull == false)
                        {
                            tiledata.rightTile.terrainType = cType;
                            tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                        }
                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd2 >= 6)
                {
                    if (tiledata.isrightTileNull == false)
                    {
                        tiledata.rightTile.terrainType = cType;
                        tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd3 >= 6)
                {
                    if (tiledata.isupTileNull == false)
                    {
                        tiledata.upTile.terrainType = cType;
                        tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd4 >= 6)
                {
                    if (tiledata.isdownTileNull == false)
                    {
                        tiledata.downTile.terrainType = cType;
                        tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                }
            }
            #endregion
            #region Gold
            else if (tiledata.terrainType == TileTypes.TerrainType.Gold)
            {

                TileTypes.TerrainType cType = TileTypes.TerrainType.Gold;
                Material cMat = tilePrefabs.goldMat;
                int rnd1 = Random.Range(0, 10);
                int rnd2 = Random.Range(0, 10);
                int rnd3 = Random.Range(0, 10);
                int rnd4 = Random.Range(0, 10);

                if (rnd1 >= 6)
                {
                    if (tiledata.isleftTileNull == false)
                    {
                        tiledata.leftTile.terrainType = cType;
                        tiledata.leftTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd2 >= 6)
                    {
                        if (tiledata.isrightTileNull == false)
                        {
                            tiledata.rightTile.terrainType = cType;
                            tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                        }
                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd2 >= 6)
                {
                    if (tiledata.isrightTileNull == false)
                    {
                        tiledata.rightTile.terrainType = cType;
                        tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd3 >= 6)
                {
                    if (tiledata.isupTileNull == false)
                    {
                        tiledata.upTile.terrainType = cType;
                        tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd4 >= 6)
                {
                    if (tiledata.isdownTileNull == false)
                    {
                        tiledata.downTile.terrainType = cType;
                        tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                }
                #endregion
                #region Diamond
            }
            else if (tiledata.terrainType == TileTypes.TerrainType.Diamond)
            {

                TileTypes.TerrainType cType = TileTypes.TerrainType.Diamond;
                Material cMat = tilePrefabs.diamondMat;
                int rnd1 = Random.Range(0, 10);
                int rnd2 = Random.Range(0, 10);
                int rnd3 = Random.Range(0, 10);
                int rnd4 = Random.Range(0, 10);

                if (rnd1 >= 6)
                {
                    if (tiledata.isleftTileNull == false)
                    {
                        tiledata.leftTile.terrainType = cType;
                        tiledata.leftTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd2 >= 6)
                    {
                        if (tiledata.isrightTileNull == false)
                        {
                            tiledata.rightTile.terrainType = cType;
                            tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                        }
                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd2 >= 6)
                {
                    if (tiledata.isrightTileNull == false)
                    {
                        tiledata.rightTile.terrainType = cType;
                        tiledata.rightTile.GetComponent<MeshRenderer>().material = cMat;


                    }
                    else if (rnd3 >= 6)
                    {
                        if (tiledata.isupTileNull == false)
                        {
                            tiledata.upTile.terrainType = cType;
                            tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd3 >= 6)
                {
                    if (tiledata.isupTileNull == false)
                    {
                        tiledata.upTile.terrainType = cType;
                        tiledata.upTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                    else if (rnd4 >= 6)
                    {
                        if (tiledata.isdownTileNull == false)
                        {
                            tiledata.downTile.terrainType = cType;
                            tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                        }
                    }
                }
                else if (rnd4 >= 6)
                {
                    if (tiledata.isdownTileNull == false)
                    {
                        tiledata.downTile.terrainType = cType;
                        tiledata.downTile.GetComponent<MeshRenderer>().material = cMat;

                    }
                }
            }
            #endregion

        }
        // Debug.Log("Finished Vein Gen In Chunk: " + chunkID);
        chunkManager.finishVeinGen();
    }



    #endregion
    #region GenTrees
    public void GenerateTrees()
    {   //adds trees to the terrain
        // Debug.Log("Generating Trees For Chunk:" + chunkID);
        for (int i = 0; i < surfaceTiles.Count; i++)
        {
            if (biomeType == TileTypes.BiomeType.Plains)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Dirt || tiles[i].terrainType == TileTypes.TerrainType.Grass)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 10)
                        {
                            int treeHeight = Random.Range(3, 15);
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            Destroy(oldt.gameObject);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y - 1), tilePrefabs.saplingTile[0]);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.saplingType = 0;
                            newt.isSapling = true;
                            newt.UpdateTileSides();
                            newt.treeHeight = treeHeight;
                        }
                    }
                }
            }
            else if (biomeType == TileTypes.BiomeType.Forest)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Dirt || tiles[i].terrainType == TileTypes.TerrainType.Grass)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 30)
                        {
                            int treeHeight = Random.Range(3, 15);
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            Destroy(oldt.gameObject);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y - 1), tilePrefabs.saplingTile[0]);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.saplingType = 0;
                            newt.isSapling = true;
                            newt.UpdateTileSides();
                            newt.treeHeight = treeHeight;
                        }
                    }
                }
            }
            else if (biomeType == TileTypes.BiomeType.Mountain)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Dirt || tiles[i].terrainType == TileTypes.TerrainType.Grass)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 20)
                        {
                            int treeHeight = Random.Range(3, 15);
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            Destroy(oldt.gameObject);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y - 1), tilePrefabs.saplingTile[0]);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.saplingType = 0;
                            newt.isSapling = true;
                            newt.UpdateTileSides();
                            newt.treeHeight = treeHeight;
                        }
                    }
                }
            }
            else if (biomeType == TileTypes.BiomeType.Snow)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Snow)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 10)
                        {
                            int treeHeight = Random.Range(3, 15);
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y - 1), tilePrefabs.saplingTile[1]);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.saplingType = 1;
                            newt.isSapling = true;
                            newt.UpdateTileSides();
                            newt.treeHeight = treeHeight;
                        }
                    }
                }

            }
            else if (biomeType == TileTypes.BiomeType.Tundra)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Snow)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 30)
                        {
                            int treeHeight = Random.Range(3, 15);
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y), tilePrefabs.saplingTile[1]);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.saplingType = 1;
                            newt.isSapling = true;
                            newt.UpdateTileSides();
                            newt.treeHeight = treeHeight;
                        }
                    }
                }

            }


            //Debug.Log("Finished Tree Gen Chunk: " + chunkID);
            chunkManager.finishTreeGen();
        }
    }
    #endregion
    #region GenerateFlowers
    public void GenerateFoliage()
    {
        //Debug.Log("Generating Flowers For Chunk:" + chunkID);
        for (int i = 0; i < surfaceTiles.Count; i++)
        {
            if (biomeType == TileTypes.BiomeType.Plains)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Grass)
                    {

                        int rnd = Random.Range(0, 100);//generate flowers
                        int rnd2 = Random.Range(0, (tilePrefabs.FlowerTile.Count - 1));

                        if (rnd <= 5)
                        {
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            //Debug.Log("Rnd2 = " + rnd2);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y), tilePrefabs.FlowerTile[rnd2]);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.terrainType = TileTypes.TerrainType.Flower;
                            newTileGO.GetComponent<BoxCollider2D>().isTrigger = true;
                            newt.UpdateTileSides();
                        }
                        else if (rnd > 5 && rnd <= 20)
                        { // generate grass
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y), tilePrefabs.GrassFoliageTile);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.terrainType = TileTypes.TerrainType.GrassFoliage;
                            newTileGO.GetComponent<BoxCollider2D>().isTrigger = true;
                            newt.UpdateTileSides();
                        }
                    }
                }
            }
            if (biomeType == TileTypes.BiomeType.Desert)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Sand)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 5)
                        {
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y), tilePrefabs.DeadBushTile);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.terrainType = TileTypes.TerrainType.DeadBush;
                            newTileGO.GetComponent<BoxCollider2D>().isTrigger = true;
                            newt.UpdateTileSides();
                        }
                    }
                }
            }
            if (biomeType == TileTypes.BiomeType.Mountain)
            {
                if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
                {
                    if (tiles[i].terrainType == TileTypes.TerrainType.Dirt)
                    {

                        int rnd = Random.Range(0, 100);
                        if (rnd <= 10)
                        {
                            Tile oldt = tiles[i];
                            tiles.Remove(oldt);
                            GameObject newTileGO = SpawnTile2((oldt.x - 1), (oldt.y), tilePrefabs.BrambleTile);
                            Tile newt = newTileGO.GetComponent<Tile>();
                            newt.terrainType = TileTypes.TerrainType.Bramble;
                            newTileGO.GetComponent<BoxCollider2D>().isTrigger = true;
                            newt.UpdateTileSides();
                            newt.updateOtherBlocks();
                        }
                    }
                }
            }
        }
        //Debug.Log("Finished Generating Flowers For Chunk:" + chunkID);
        chunkManager.finishFlowerGen();
    }
    #endregion
    #region GenLakes
    public void GenerateLakes()
    {

        for (int i = 0; i < surfaceTiles.Count; i++)
        {
            if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
            {
                int rnd = Random.Range(0, 100);
                if (rnd <= 1)
                {

                    int cloudDepth = Random.Range(3, 7);
                    int cloudLength;
                    int prevCloudLength = 200;
                    int repeats = 0;
                    for (int j = 0; j < cloudDepth; j++)
                    {

                        if (repeats < 20)
                        {
                            cloudLength = Random.Range(5, 15);
                            if (cloudLength < prevCloudLength)
                            {
                                int cloudBalance = 0;
                                prevCloudLength = cloudLength;
                                //prevCloudLength = cloudLength;
                                do
                                {
                                    Tile oldt = tiles[i];
                                    GameObject newTileGO;
                                    switch (cloudBalance)
                                    {
                                        case 0:
                                            newTileGO = SpawnTile2((oldt.x - cloudLength), (oldt.y + (30 - j)), tilePrefabs.CloudTile);
                                            //newTileGO.GetComponent<Tile> ().isCloud = true;
                                            cloudBalance = 1;
                                            break;
                                        case 1:
                                            newTileGO = SpawnTile2((oldt.x + cloudLength), (oldt.y + (30 - j)), tilePrefabs.CloudTile);
                                            //newTileGO.GetComponent<Tile> ().isCloud = true;
                                            cloudLength--;
                                            cloudBalance = 0;
                                            break;
                                        default:
                                            newTileGO = SpawnTile2((oldt.x - cloudLength), (oldt.y + (30 - j)), tilePrefabs.CloudTile);
                                            //newTileGO.GetComponent<Tile> ().isCloud = true;
                                            cloudBalance = 0;
                                            break;
                                    }
                                    Tile newt = newTileGO.GetComponent<Tile>();
                                    newt.terrainType = TileTypes.TerrainType.Cloud;
                                    newTileGO.GetComponent<BoxCollider2D>().isTrigger = true;
                                    newt.UpdateTileSides();
                                } while (cloudLength >= 0);
                            }
                            else { j--; repeats++; }
                        }
                    }
                }
            }
        }
        //for (int i = 0; i < tiles.Count; i++)
        //{
        //    if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
        //    {
        //        if (tiles[i].terrainType == TileTypes.TerrainType.Grass)
        //        {

        //            int rnd = Random.Range(0, 100);
        //            if (rnd <= 1)
        //            {

        //                int lakeDepth = Random.Range(3, 10);
        //                int lakeLength;
        //                int prevLakeLength = 200;
        //                int repeats = 0;
        //                for (int j = 0; j < lakeDepth; j++)
        //                {

        //                    if (repeats < 20)
        //                    {
        //                        lakeLength = Random.Range(5, 15);
        //                        if (lakeLength < prevLakeLength)
        //                        {
        //                            int lakeBalance = 0;
        //                            prevLakeLength = lakeLength;
        //                            //prevCloudLength = cloudLength;
        //                            do
        //                            {
        //                                Tile oldt = tiles[i];
        //                                GameObject newTileGO;
        //                                switch (lakeBalance)
        //                                {
        //                                    case 0:

        //                                        newTileGO = SpawnTile2((oldt.x - lakeLength), (oldt.y - (j + 1)), tilePrefabs.waterTile);
        //                                        newTileGO.GetComponent<Tile>().isCloud = true;
        //                                        lakeBalance = 1;
        //                                        break;
        //                                    case 1:

        //                                        newTileGO = SpawnTile2((oldt.x + lakeLength), (oldt.y - (j + 1)), tilePrefabs.waterTile);
        //                                        newTileGO.GetComponent<Tile>().isCloud = true;
        //                                        lakeLength--;
        //                                        lakeBalance = 0;
        //                                        break;
        //                                    default:
        //                                        newTileGO = SpawnTile2((oldt.x - lakeLength), (oldt.y - (j + 1)), tilePrefabs.waterTile);
        //                                        newTileGO.GetComponent<Tile>().isCloud = true;
        //                                        lakeBalance = 0;
        //                                        break;
        //                                }
        //                                Tile newt = newTileGO.GetComponent<Tile>();
        //                                newt.terrainType = TileTypes.TerrainType.Water;
        //                                newTileGO.GetComponent<BoxCollider2D>().isTrigger = true;
        //                                newt.UpdateTileSides();
        //                            } while (lakeLength >= 0);
        //                        }
        //                        else { j--; repeats++; }
        //                    }
        //                }
        //            }
        //        }
        //   }
        //  }
        chunkManager.finishLakeGen();
    }


    #endregion
    #region GenerateSpawnPoint
    public void GenSpawnPoint()
    {
        bool chosenYet = false;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].isupTileNull == true && tiles[i].isBackground == false)
            {
                if (tiles[i].terrainType == TileTypes.TerrainType.Grass || tiles[i].terrainType == TileTypes.TerrainType.Dirt || tiles[i].terrainType == TileTypes.TerrainType.Sand)
                {
                    if (chosenYet)
                    {

                    }
                    else
                    {
                        chosenYet = true;
                        chunkManager.SpawnPoint = tiles[i];
                        chunkManager.finishSpawnPointGen();
                    }
                }
            }
        }

        if (chosenYet == false)
        {
            chunkManager.chunks[chunkID + 1].GetComponent<GenerateChunk>().GenSpawnPoint();
        }

    }
    #endregion

    #region Util
    public Tile getTileByGlobalCoords(int globalx, int globaly)
    {
        foreach (Tile _t in chunkManager.globalTileList)
        {
            if (_t.GlobalTileX == globalx && _t.GlobalTileY == globaly)
            {
                return _t;
            }
            else
            {
                return null;
            }

        }
        return null;
    }
    #endregion
    #region Ienumerators
    IEnumerator wait1()
    {

        yield return new WaitForSeconds(1);

    }
    IEnumerator wait2()
    {

        yield return new WaitForSeconds(5);

    }

    #endregion

}
