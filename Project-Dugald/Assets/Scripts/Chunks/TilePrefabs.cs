using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefabs : MonoBehaviour {

    //Ores
    public GameObject coalTile;
    public GameObject ironTile;
    public GameObject goldTile;
    public GameObject diamondTile;
    //Natural Blocks
    public GameObject dirtTile;
    public GameObject stoneTile;
    public GameObject grassTile;
    public GameObject sandstoneTile;
    public GameObject sandTile;
    public GameObject waterTile;
	public GameObject snowTile;
	public GameObject snowSlope1Tile;
	public GameObject snowSlope2Tile;
	public GameObject CloudTile;
    //Manmade Blocks
    public GameObject woodPlankTile;
    //Machines
    public GameObject workbench;

    //Foliage
    public GameObject[] saplingTile;
    public GameObject[] leaveTile;
    public GameObject[] trunkTile;
    public List<GameObject> FlowerTile = new List<GameObject>(); 
    public GameObject GrassFoliageTile;
	public GameObject DeadBushTile; 
	public GameObject BrambleTile;

    public Material stoneBackgroundMat;
    public Material dirtBackgroundMat;
    public Material sandBackgroundMat;

    public Material coalMat;
    public Material ironMat;
    public Material diamondMat;
    public Material goldMat;
    public Material errorMat;

    public Material waterSlope1Mat;
    public Material waterSlope2Mat;

    public GameObject droppedBagPrefab;

}
