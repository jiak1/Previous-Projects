using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColourMap, Mesh, Falloff };
    public DrawMode drawMode;
    public Noise.NormalizeMode normalizeMode;
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int editorPreviewLOD;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool useFalloff;
    public bool randomIslands;
    [Range(0, 1)]
    public float islandChance;


    public bool generateGameObjects;
    public bool generateClouds;
    public bool generateWater;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool anyBiome;
    public Utility.Biome biomeToForce;

    public bool autoUpdate;

    public BiomeRegion[] regions;

    public BiomeGameObjects[] objectsToGenerate;
    public GameObject cloud;

    //private GenerateGameObjects genGOs;

    float[,] falloffMap;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private void Awake()
    {
        //genGOs = GetComponent<GenerateGameObjects>();
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD), TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
            if (generateGameObjects)
            {
                GenerateGameObjects genGOScript = display.meshRenderer.transform.gameObject.GetComponent<GenerateGameObjects>();
                if (genGOScript == null) { genGOScript = display.meshRenderer.transform.gameObject.AddComponent<GenerateGameObjects>(); }
                genGOScript.seed = seed;
                genGOScript.StartGenerateObjects();
                genGOScript.offset = offset;
                genGOScript.levelOfDetail = editorPreviewLOD;
                genGOScript.mapData = mapData;
                genGOScript.genClouds = generateClouds;
            }
        }
        else if (drawMode == DrawMode.Falloff)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }

        if (generateWater)
        {
            bool genNewWater = true;
            foreach (Transform child in gameObject.transform)
            {
                if(child.tag == "Water")
                {
                    genNewWater = false;
                }
            }
            if (genNewWater)
            {
                EndlessTerrain endTer = GetComponent<EndlessTerrain>();
                GameObject waterIns = GameObject.Instantiate(endTer.waterPlane, new Vector3(0, -0.22f * EndlessTerrain.scale, 0), Quaternion.identity);
                waterIns.transform.parent = gameObject.transform;
                waterIns.AddComponent<DisableOnStartup>();
            }
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, center + offset, normalizeMode);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        Utility.Biome islandBiome;
        if (anyBiome)
        {
            float biomePerlin = Noise.GenerateNoise(seed, center + offset);
            islandBiome = Utility.GetBiomeFromPerlin(biomePerlin);
        }
        else
        {
            islandBiome = biomeToForce;
        }

        BiomeRegion selectedRegion = new BiomeRegion();
        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i].biome == islandBiome)
            {
                selectedRegion = regions[i];
            }
        }

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    if (randomIslands)
                    {
                        float noise = Noise.GenerateNoise(seed, new Vector2(center.x + offset.x+seed,center.y+offset.y/seed));
                        //print(noise);
                        if (noise >= islandChance)
                        {
                            //We have just water
                            float tempFloat = 0.1f;
                            noiseMap[x, y] = tempFloat;
                        }
                        else
                        {
                            //We have normal island
                            noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] - falloffMap[x, y], 0, 1);
                        }
                    }
                    else
                    {
                        noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] - falloffMap[x, y], 0, 1);
                    }
                }
                float currentHeight = noiseMap[x, y];

                if (selectedRegion.terrainTypes != null)
                {
                    for (int i = 0; i < selectedRegion.terrainTypes.Length; i++)
                    {
                        if (currentHeight >= selectedRegion.terrainTypes[i].height)
                        {
                                colourMap[y * mapChunkSize + x] = selectedRegion.terrainTypes[i].colour;
                            
                        }
                        //else
                        //{
                        //    break;
                        //}
                    }
                }
            }
        }
        MapData mapData = new MapData(noiseMap, colourMap);
        return mapData;
    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (islandChance == 0)
        {
            islandChance = 0.5f;
        }
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }

    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

}

[System.Serializable]
public struct BiomeRegion
{
    public string name;
    public Utility.Biome biome;
    public TerrainType[] terrainTypes;

}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}