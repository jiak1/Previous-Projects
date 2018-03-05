using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{


    public static float genPerlin(float x, float y, int seed)
    {

        float newY = ((float)y + (float)seed + 0.01f);
        float newX = ((float)x + (float)seed + 0.01f);
        float perlin = Mathf.PerlinNoise(newX * newY, 0.1f);


        return perlin;
    }

    public static Tile.TileType getTileFromPerlin(float perlin)
    {
        if (perlin <= 0.23f)
        {
            return Tile.TileType.Water;
        }
        else if (perlin <= 0.3f)
        {
            return Tile.TileType.Sand;
        }
        else if (perlin <= 0.7f)
        {
            return Tile.TileType.Grass_Flat;
        }
        else
        {
            return Tile.TileType.Stone;
        }
    }

    public static GameObject getGameObjectFromFile(Tile.TileType tileType)
    {
        if (tileType == Tile.TileType.Grass_Flat)
        {
            return Resources.Load<GameObject>("Grass_Flat_Tile");
        }
        else if (tileType == Tile.TileType.Water)
        {
            return Resources.Load<GameObject>("Water_Tile");
        }
        else if (tileType == Tile.TileType.Sand)
        {
            return Resources.Load<GameObject>("Sand_Tile");
        }
        else if (tileType == Tile.TileType.Stone)
        {
            return Resources.Load<GameObject>("Stone_Hill_Tile");
        }
        else
        {
            return null;
        }
    }

    public static GameObject getGameObjectFromFile(Resource.ResourceType resourceType)
    {
        if (resourceType == Resource.ResourceType.Coal)
        {
            return Resources.Load<GameObject>("Coal_Resource");
        }
        else if (resourceType == Resource.ResourceType.Iron)
        {
            return Resources.Load<GameObject>("Iron_Resource");
        }
        else if (resourceType == Resource.ResourceType.Copper)
        {
            return Resources.Load<GameObject>("Copper_Resource");
        }
        else if (resourceType == Resource.ResourceType.Gold)
        {
            return Resources.Load<GameObject>("Gold_Resource");
        }
        else if (resourceType == Resource.ResourceType.Tin)
        {
            return Resources.Load<GameObject>("Tin_Resource");
        }
        else if (resourceType == Resource.ResourceType.Silver)
        {
            return Resources.Load<GameObject>("Silver_Resource");
        }
        else
        {
            return null;
        }
    }

    public static bool checkGenResource(int x, int y, float[,] genResourceNoiseMap, Tile.TileType tileType)
    {
        if (tileType == Tile.TileType.Dirt_Flat || tileType == Tile.TileType.Grass_Flat || tileType == Tile.TileType.Stone)
        {

            if (genResourceNoiseMap[x, y] >= 0.75f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static Resource.ResourceType getUnderGroundResource(int x, int y, float[,] resourceNoiseMap)
    {

        if (resourceNoiseMap[x, y] >= 0.85f)
        {
            return Resource.ResourceType.Gold;
        }
        else if (resourceNoiseMap[x, y] >= 0.7f)
        {
            return Resource.ResourceType.Copper;
        }
        else if (resourceNoiseMap[x, y] >= 0.65f)
        {
            return Resource.ResourceType.Silver;

        }
        else if (resourceNoiseMap[x, y] >= 0.55f)
        {
            return Resource.ResourceType.Tin;
        }
        else if (resourceNoiseMap[x, y] >= 0.4f)
        {
            return Resource.ResourceType.Iron;
        }
        else
        {
            return Resource.ResourceType.Coal;
        }
    }

    public static float getYHeight(Tile.TileType type)
    {
        if (type == Tile.TileType.Dirt_Hill || type == Tile.TileType.Grass_Hill || type == Tile.TileType.Stone_Hill || type == Tile.TileType.Stone)
        {
            return 2f;
        }
        else
        {
            return 1f;
        }
    }

    public static Color getResourceColor(Resource.ResourceType type)
    {
        if(type == Resource.ResourceType.Coal)
        {
            return Color.black;
        }else if(type == Resource.ResourceType.Copper)
        {
            return new Color(219, 177, 1, 1);
        }
        else if (type == Resource.ResourceType.Gold)
        {
            return Color.yellow;
        }
        else if (type == Resource.ResourceType.Iron)
        {
            return new Color(218, 208, 178, 1);
        }
        else if (type == Resource.ResourceType.Silver)
        {
            return new Color(208, 208, 208, 1);
        }
        else if (type == Resource.ResourceType.Tin)
        {
            return Color.white;
        }
        else
        {
            return Color.magenta;
        }
    }

}
