using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public int seed;
    int mapWidth;
    int mapHeight;
    public int chunkSize = 100;
    public float noiseScale;

    public Vector2 offset;
    public Vector2 noiseOffset;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public bool autoUpdate;

    public GameObject player;

    Chunk chunk;
	// Use this for initialization
	void Start () {
        mapWidth = chunkSize;
        mapHeight = chunkSize;
        //seed = Random.Range(-100000, 100000);

        InstantiateChunk(new Vector2(0, 0));
        InstantiateChunk(new Vector2(1, 0));
        InstantiateChunk(new Vector2(-1, 0));
        InstantiateChunk(new Vector2(0, 1));
        InstantiateChunk(new Vector2(0, -1));
        InstantiateChunk(new Vector2(-1, -1));
        InstantiateChunk(new Vector2(1, 1));

    }
	
    void InstantiateChunk(Vector2 offset)
    {
        Vector2 _noiseOffset = new Vector2(offset.x * noiseOffset.x, offset.y * noiseOffset.y);
        GameObject chunkGO = Resources.Load<GameObject>("Chunk");
        GameObject chunkObject = Instantiate(chunkGO,this.transform);
        Chunk chunkScript = chunkObject.AddComponent<Chunk>();
        chunkScript.GenChunk(_noiseOffset, this);
        chunkObject.transform.position = new Vector3(offset.x * chunkSize, 1, offset.y * chunkSize);
        chunkObject.name = "Chunk: X:" + offset.x + " Y:" + offset.y;
    }

    public void GenerateNoiseMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, noiseScale,octaves,persistance,lacunarity,seed,offset);

        NoisePreview noisePreview = FindObjectOfType<NoisePreview>();
        noisePreview.DrawNoiseMap(noiseMap);
    }
	// Update is called once per frame
	void OnValidate () {
		if(chunkSize < 1)
        {
            chunkSize = 1;
        }

        if(lacunarity < 1)
        {
            lacunarity = 1;

        }
        if(octaves < 0)
        {
            octaves = 0;
        }

	}

}
