using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes{

    public enum TerrainType
    {
        Error,
        Grass,
        Stone,
        Dirt,
        Sand,
        Sandstone,
        Water,
        Air,
		Cloud,
       //foilage
		Trunk,
		Leave,
		Sapling,
        Snow,
		Flower,
		GrassFoliage, 
		DeadBush,
		Bramble,
        //Manmade Blocks
        WoodPlanks,
        //Machines
        Workbench,
        //Ores
        Gold,
        Iron,
        Coal,
        Diamond,

    }

    public enum BiomeType
    {
		Plains, 
        Forest, 
        Tundra, 
        Desert, 
        Snow,	
        Ice,	
        Ocean,	
        Mountain
    }
}
