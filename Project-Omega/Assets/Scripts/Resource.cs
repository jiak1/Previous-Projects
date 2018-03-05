using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {

    public enum ResourceType { Iron,Coal,Copper,Tin,Silver,Gold}
    public ResourceType resourceType;
    public Tile tile;
    public float yield;
    public GameObject prefab;
    public int x;
    public int y;

    public Resource(int _x, int _y, GameObject _prefab, Tile _tile, ResourceType _resourceType, float _yield = 1f)
    {
        x = _x;
        y = _y;
        prefab = _prefab;
        tile = _tile;
        resourceType = _resourceType;
        yield = _yield;

        float scale = (_yield * 10)*4f;
        Transform model = null;
        foreach (Transform _t in prefab.transform)
        {
            model = _t;
        }
        if(model != null)
        {
            model.transform.localScale = new Vector3(model.transform.localScale.x, scale, model.transform.localScale.z);
        }
    }
}
