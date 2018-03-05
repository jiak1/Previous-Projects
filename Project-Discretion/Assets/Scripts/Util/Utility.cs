using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    public enum Biome { Forest, AspenForest, Desert, Rock, NULL }

    public static Biome GetBiomeFromPerlin(float perlinNoise)
    {
        if (perlinNoise >= 0.5f)
        {
            return Biome.AspenForest;
        }else if(perlinNoise > 0.45f && perlinNoise < 0.5f)
        {
            return Biome.Rock;
        }else if(perlinNoise > 0.4f && perlinNoise <= 0.45f)
        {
            return Biome.Desert;
        }
        else if (perlinNoise <= 0.4f)
        {
            return Biome.Forest;
        }
        else
        {
            return Biome.NULL;
        }
    }
}
