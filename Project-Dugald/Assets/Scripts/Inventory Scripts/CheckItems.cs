using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItems : MonoBehaviour {


    public bool CheckIfItemIsBlock(ItemList.ItemType _item)
    {
        if(_item == ItemList.ItemType.Log) { return true; }
        if (_item == ItemList.ItemType.Workbench) { return true; }
        if (_item == ItemList.ItemType.WoodenPlanks) { return true; }
        return false;
    }

    public GameObject ReturnItemTilePrefab(TilePrefabs _tilePref, ItemList.ItemType _item)
    {
        if(_item == ItemList.ItemType.Log) { return _tilePref.trunkTile[0]; }
        if (_item == ItemList.ItemType.Workbench) { return _tilePref.workbench; }
        if (_item == ItemList.ItemType.WoodenPlanks) { return _tilePref.woodPlankTile; }
        return null;
    }
}
