using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCreator : MonoBehaviour
{

    [HideInInspector]
    public GameObject cloud;

    [HideInInspector]
    public Vector2 offset;
    [HideInInspector]
    public int seed;

    private List<GameObject> spawnedClouds = new List<GameObject>();

    public void GenClouds()
    {
        if(spawnedClouds.Count > 0) { RemoveClouds(); };
        seed = GetComponent<GenerateGameObjects>().seed;
        offset = GetComponent<GenerateGameObjects>().offset;
        cloud = GameObject.FindObjectOfType<MapGenerator>().GetComponent<MapGenerator>().cloud;

        float perlin = Noise.GenerateNoise(seed, offset);
        if (Mathf.RoundToInt(perlin) >= 1)
        {
            int amountOfClouds;
            if (perlin >= 0.9) { amountOfClouds = 1; } else if (perlin >= 0.7) { amountOfClouds = 2; } else if (perlin >= 0.5) { amountOfClouds = 4; } else if (perlin >= 0.2) { amountOfClouds = 6; } else { amountOfClouds = 8; };
            for (int i = 0; i < amountOfClouds; i++)
            {
                GameObject _cloud = Instantiate(cloud, new Vector3(transform.position.x,transform.position.y + 40,transform.position.z), Quaternion.identity, transform);
                spawnedClouds.Add(_cloud);
            }
        }
    }

    void RemoveClouds()
    {
        foreach (GameObject _cloud in spawnedClouds)
        {

            spawnedClouds.Remove(_cloud);
            DestroyImmediate(_cloud);
        }
    }

}
