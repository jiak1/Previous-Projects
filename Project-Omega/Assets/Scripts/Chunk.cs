using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    Tile[,] tiles;

    int seed;
    int resourceSeed;
    int genResourcesSeed;
    int resourcesYieldSeed;

    int width;
    int height;
    float scale;
    float persistance;
    float lacunarity;
    int octaves;
    Transform parent;
    public Vector2 noiseOffset; 
    MapController mapController;

    Resource[,] resources;
    public bool autoUpdate;

    public void GenChunk(Vector2 offset,MapController _mapController)
    {
        resourceSeed = Random.Range(-100000, 100000);
        genResourcesSeed = Random.Range(-100000, 100000);
        resourcesYieldSeed = Random.Range(-100000, 100000);
        noiseOffset = offset;
        mapController = _mapController;
        parent = this.transform;
        seed = mapController.seed;
        width = mapController.chunkSize;
        height = mapController.chunkSize;
        scale = mapController.noiseScale;
        persistance = mapController.persistance;
        lacunarity = mapController.lacunarity;
        octaves = mapController.octaves;
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, seed, offset);
        float[,] underGroundResourcesNoiseMap = Noise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity, resourceSeed, offset);
        float[,] underGroundResourcesYieldNoiseMap = Noise.GenerateNoiseMap(width, height, 7f, octaves, persistance, lacunarity, resourcesYieldSeed, offset);
        float[,] genResourcesNoiseMap = Noise.GenerateNoiseMap(width, height, 10f, octaves, persistance, lacunarity, genResourcesSeed, offset);
        tiles = new Tile[width, height];
        resources = new Resource[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y, Util.getTileFromPerlin(noiseMap[x, y]));
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject objectPrefab = Util.getGameObjectFromFile(GetTileAt(x, y).type);
                GameObject tile_GO = GameObject.Instantiate(objectPrefab, parent, true);
                tile_GO.name = "Tile_" + x + "_" + y;
                tile_GO.transform.position = new Vector3(x, Util.getYHeight(tiles[x, y].type), y);
                bool boolGenResource = Util.checkGenResource(x, y, genResourcesNoiseMap, tiles[x, y].type);
                if (boolGenResource == true)
                {
                    Resource.ResourceType type = Util.getUnderGroundResource(x, y,underGroundResourcesNoiseMap);
                    GameObject prefab = Util.getGameObjectFromFile(type);
                    GameObject resource_GO = GameObject.Instantiate(prefab, parent, true);
                    resource_GO.name = "Resource: " + type.ToString();
                    resource_GO.transform.position = new Vector3(x, Util.getYHeight(tiles[x,y].type), y);
                    resources[x, y] = new Resource(x,y,prefab,tiles[x,y],type,underGroundResourcesYieldNoiseMap[x,y]);
                }
                GetTileAt(x, y).Prefab = tile_GO;
            }
        }

        //Debug.Log("Chunk Created, Width: " + width + ", Height: " + height + ", Seed: " + seed);
    }

    public Tile GetTileAt(int x, int y)
    {
        if (tiles[x, y] == null)
        {
            Debug.LogError("Tile" + x + "," + y + " is null");
            return null;
        }


        return tiles[x, y];
    }


}
