using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndlessTerrain : MonoBehaviour
{

    public const float scale = 10;
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    Vector2 viewerPoisitionOld;

    public LODInfo[] detailLevels;
    public static float maxViewDst = 450;
    public Transform viewer;
    public Material mapMaterial;
    public GameObject waterPlane;
    private GenerateGameObjects genGOs;

    public bool canStart = false;

    public static Vector2 viewerPosition;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {
        genGOs = GetComponent<GenerateGameObjects>();
        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        UpdateVisibleChunks();
    }

    void Update()
    {
        if (viewer != null)
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;

            if ((viewerPoisitionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
            {
                viewerPoisitionOld = viewerPosition;
                UpdateVisibleChunks();
            }

        }
    }

    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial, waterPlane, genGOs));
                }

            }
        }
    }
    public class TerrainChunk
    {
        MeshCollider meshCollider;
        GenerateGameObjects genGO;
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;
        GameObject waterIns;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        LODMesh collisionMesh;

        GenerateGameObjects genGOs;
        MapData mapData;
        bool mapDataRecieved;
        int previousLODIndex = -1;
        Vector3 positionV3;
        bool isIsland = true;
        bool doneGOGen = false;
        GameObject waterPlane;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material, GameObject _waterPlane, GenerateGameObjects genGOs)
        {
            waterPlane = _waterPlane;
            this.detailLevels = detailLevels;
            position = coord * size;

            if (mapGenerator.randomIslands)
            {
                float noise = Noise.GenerateNoise(mapGenerator.seed, new Vector2(position.x + mapGenerator.offset.x + mapGenerator.seed, position.y + mapGenerator.offset.y / mapGenerator.seed));
                //print(noise);
                if (noise >= mapGenerator.islandChance)
                {
                    //It is just water and not an island
                    isIsland = false;
                }
                else { isIsland = true; }
            }

            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk, X: " + position.x + ", Y: " + position.y);
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;
            meshCollider = meshObject.AddComponent<MeshCollider>();

            if (waterIns == null && mapGenerator.generateWater)
            {
                Vector3 _position = new Vector3(0, -0.22f * scale, 0);
                if (isIsland)
                {
                    _position.y = -6.6f;
                }
                waterIns = GameObject.Instantiate(waterPlane, _position, Quaternion.identity);
                waterIns.transform.parent = meshObject.transform;
            }
            meshObject.transform.position = positionV3 * scale;
            meshObject.transform.parent = parent;
            if (isIsland == false) { meshObject.AddComponent<FixHeights>(); }

            meshObject.transform.localScale = Vector3.one * scale;

            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
                if (detailLevels[i].useForCollider)
                {
                    collisionMesh = lodMeshes[i];
                }
            }

            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData)
        {
            this.mapData = mapData;
            mapDataRecieved = true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }


        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDst;
            meshObject.transform.position.y.Equals(-44f);
            if (mapDataRecieved)
            {

                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {

                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (lodIndex != previousLODIndex)
                    {
                        if (genGO != null) { genGO.levelOfDetail = lodIndex; }
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                            if (mapGenerator.generateGameObjects && doneGOGen == false)
                            {
                                genGO = meshObject.AddComponent<GenerateGameObjects>();
                                genGO.seed = mapGenerator.seed;
                                genGO.offset = position;
                                genGO.mapData = mapData;
                                genGO.scale = scale;
                                genGO.genClouds = mapGenerator.generateClouds;
                                genGO.genWater = mapGenerator.generateWater;
                                genGO.isIsland = isIsland;
                                genGO.waterPlane = waterPlane;
                                genGO.StartGenerateObjects();
                                if (previousLODIndex != -1)
                                { genGO.levelOfDetail = previousLODIndex; }
                                else { genGO.levelOfDetail = 0; }
                                doneGOGen = true;
                            }
                            if (doneGOGen)
                            {
                                genGO.StartGenerateObjects();
                            }
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }

                    if(lodIndex == 0)
                    {
                        if (collisionMesh.hasMesh)
                        {
                            meshCollider.sharedMesh = collisionMesh.mesh;
                        }else if (collisionMesh.hasRequestedMesh)
                        {
                            collisionMesh.RequestMesh(mapData);
                        }
                    }

                    terrainChunksVisibleLastUpdate.Add(this);
                }

            }
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            if (waterIns != null) { waterIns.SetActive(visible); }
            meshObject.SetActive(visible);

    }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }

    }

    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;

        System.Action updateCallback;

        void OnMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
            updateCallback();
        }

        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        public void RequestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDstThreshold;
        public bool useForCollider;
    }
}