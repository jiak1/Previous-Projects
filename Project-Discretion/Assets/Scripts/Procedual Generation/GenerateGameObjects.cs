using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGameObjects : MonoBehaviour
{
    [HideInInspector]
    public float scale = 1;
    [SerializeField]
    public Utility.Biome biome;

    public Vector2 offset;
    private MapGenerator mapGen;
    public BiomeGameObjects[] objectsToGenerate;
    [HideInInspector]
    public int seed;
    [HideInInspector]
    public MapData mapData;
    public int levelOfDetail = 0;
    private List<Vector3> takenPositions = new List<Vector3>();

    [HideInInspector]
    public bool genClouds;
    [HideInInspector]
    public bool genWater;
    [HideInInspector]
    public bool isIsland;
    [HideInInspector]
    public GameObject waterPlane;


    void GenerateWater()
    {
        Vector3 positionV3 = gameObject.transform.position;
        if (genWater)
        {
            if (isIsland)
            {
                Instantiate(waterPlane, new Vector3(0, -0.44f * scale, 0), Quaternion.identity, transform);
            }
            else
            {
                Instantiate(waterPlane, new Vector3(0, 0f * scale, 0), Quaternion.identity, transform);
            }
        }

    }

    public void StartGenerateObjects()
    {
        //if (genWater) { GenerateWater(); }
        takenPositions.Clear();
        mapGen = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MapGenerator>();
        objectsToGenerate = mapGen.objectsToGenerate;

        if (mapGen.generateClouds)
        {
            CloudCreator cloudSpawner = GetComponent<CloudCreator>();
            if (cloudSpawner == null) { cloudSpawner = transform.gameObject.AddComponent<CloudCreator>(); }
            cloudSpawner.GenClouds();
        }

        int childCount = gameObject.transform.childCount;


        if (childCount > 0)
        {
            //StartCoroutine(WaitForChildCleansing());

            List<Transform> tempList = new List<Transform>();

            foreach (Transform childTrans in gameObject.transform)
            {
                if (childTrans.tag != "Water")
                {
                    tempList.Add(childTrans);
                    }
            }

            foreach (Transform child in tempList)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(child.gameObject);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
            ContinueGenerateObjects();
        }
        else
        {
            ContinueGenerateObjects();
        }

    }

    void ContinueGenerateObjects()
    {
        Vector3 lastPos = Vector3.zero;


        mapGen = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MapGenerator>();
        Utility.Biome islandBiome;
        if (mapGen.anyBiome)
        {
            float biomePerlin = Noise.GenerateNoise(seed, offset);
            islandBiome = Utility.GetBiomeFromPerlin(biomePerlin);
        }else
        {
            islandBiome = mapGen.biomeToForce;
        }

        biome = islandBiome;
        //bool ran1 = false;
        //bool ran2 = false;
        //bool ran3 = false;
        //bool ran4 = false;

        Mesh mesh;

        if (Application.isEditor) { mesh = GetComponent<MeshFilter>().sharedMesh; } else { mesh = GetComponent<MeshFilter>().mesh; }

        Vector3[] vertices = mesh.vertices;

        int lastPlaced = 1;

        BiomeGameObjects biomeGO = new BiomeGameObjects();
        for (int x = 0; x < objectsToGenerate.Length; x++)
        {
            if (objectsToGenerate[x].biome == islandBiome)
            {
                biomeGO = objectsToGenerate[x];
            }
        }

        

        //For each object
        for (int i = 0; i < biomeGO.objects.Length; i++)
        {
            lastPos = Vector3.zero;
            lastPlaced = 1;
            //For each vertice
            for (int v = 0; v < vertices.Length; v++)
            {
                lastPlaced = lastPlaced + 1;
                //ran1 = true;
                float selectedHeight = vertices[v].y;

                Object go = biomeGO.objects[i];

                //ran2 = true;

                if (go.Generate)
                {

                    if (selectedHeight > go.minHeight && selectedHeight < go.maxHeight)
                    {
                        //ran3 = true;
                        Vector2 newOffset = new Vector2((vertices[v].x + this.transform.position.x + go.offset.x) * scale, (vertices[v].y + go.offset.y) * scale);
                        int newFloat = Mathf.RoundToInt((seed * go.randomness) / 3.4f);
                        float newPerlin = Noise.GenerateNoise(newFloat, newOffset);

                        //go.perlinValues.Add(newPerlin);
                        if (islandBiome == biomeGO.biome)
                        {
                            if ((newPerlin) * 11f < (go.spawnrate) && lastPlaced > (go.sparity * 100))
                            {
                                lastPlaced = 1;
                                //ran4 = true;
                                Vector3 treePos = new Vector3(Mathf.RoundToInt(((vertices[v].x + go.offset.x) * scale) + this.transform.position.x), Mathf.RoundToInt(((vertices[v].y) * scale) + go.offset.y), Mathf.RoundToInt(((vertices[v].z + go.offset.z) * scale) + this.transform.position.z));

                                //Vector3 treePos = new Vector3(((vertices[v].x + go.offset.x) * scale) + this.transform.position.x, ((vertices[v].y) * scale) + go.offset.y, ((vertices[v].z + go.offset.z) * scale) + this.transform.position.z);
                                if (!(takenPositions.Contains(treePos)))
                                {
                                    bool canSpawn = true;
                                    if (!(lastPos == Vector3.zero))
                                    {
                                        foreach (Vector3 _treePos in takenPositions)
                                        {
                                            if (Vector3.Distance(treePos, lastPos) < go.minDist)
                                            {
                                                canSpawn = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (canSpawn)
                                    {
                                        go.instance = Instantiate(go.prefab, treePos, go.rotation, transform);
                                        if (go.randomRotation)
                                        {

                                            go.instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                                        }
                                        go.instance.transform.localScale = new Vector3(go.scaleMultiplier, go.scaleMultiplier, go.scaleMultiplier);
                                        if(go.scaleMultiplier == 0)
                                        {
                                            Debug.Log("WARNING SCALE IS SET TO 0 "+go.name+ " OBJECT WILL BE INVISIBLE!!!!");
                                        }
                                        takenPositions.Add(treePos);
                                        lastPos = treePos;

                                    }
                                }



                            }
                        }
                    }
                }
            }
        }

        //Debug.Log(ran1.ToString()+ ran2.ToString()+ ran3.ToString()+ ran4.ToString());


    }


    public void RemovePreviousObjects()
    {

        List<Transform> tempList = new List<Transform>();

        foreach (Transform childTrans in gameObject.transform)
        {
            if (childTrans.tag != "Water"){
                tempList.Add(childTrans);
            }
        }

        foreach (Transform child in tempList)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }
}

[System.Serializable]
public struct Object
{
    public string name;

    public enum gameObjectType { Foliage, Trees, Plants, Rocks }
    [HideInInspector]
    public GameObject instance;

    public gameObjectType type;

    public GameObject prefab;

    public Vector3 offset;

    public float scaleMultiplier;

    public bool randomRotation;
    public Quaternion rotation;

    [Range(-10, 40)]
    public float minHeight;
    [Range(-10, 40)]
    public float maxHeight;
    [Range(0, 10)]
    public float spawnrate;

    [Range(0, 10)]
    public float randomness;
    [Range(0, 20)]
    public int sparity;
    [Range(0, 50)]
    public float minDist;
    public bool Generate;

    //public List<float> perlinValues ;
}

[System.Serializable]
public struct BiomeGameObjects
{
    public string name;
    public Utility.Biome biome;
    public Object[] objects;
}


