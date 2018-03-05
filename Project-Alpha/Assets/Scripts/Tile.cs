using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour{
    public enum TileType { DESERT, WATER, LAND, ICE, MOON, SUN }

    public TileType tileType;

    public int locX;
    public int locY;
    public int locZ;

    public bool hasTrees;
}
