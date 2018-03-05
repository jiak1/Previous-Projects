using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public enum TileType { Grass_Flat,Water,Sand,Grass_Hill,Stone,Stone_Hill,Dirt_Flat,Dirt_Hill};
    public TileType type;

    Chunk chunk;
    int x;
    int y;
    GameObject prefab;

    public GameObject Prefab
    {
        get
        {
            return prefab;
        }

        set
        {
            prefab = value;
        }
    }

    public Tile(Chunk chunk,int x, int y,TileType tileType)
    {
        this.chunk = chunk;
        this.x = x;
        this.y = y;
        this.type = tileType;
    }

}
