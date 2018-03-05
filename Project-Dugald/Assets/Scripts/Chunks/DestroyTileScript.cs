using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTileScript : MonoBehaviour {

    [HideInInspector]
    public Tile tile;
    public void StartDestroyingTile(Tile _tile)
    {
        tile = _tile;
        _tile.UpdateTileSides();
        _tile.DelayedSetBlockSides();

        DetermineWhatTileType(_tile);
    }

    void DetermineWhatTileType(Tile _tile)
    {
        
        //All Flowers
        if(_tile.terrainType == TileTypes.TerrainType.Flower){DropBag(_tile, ItemList.ItemType.Mango, 1, true);return;}
        //Dead Bush
        if (_tile.terrainType == TileTypes.TerrainType.DeadBush) { DropBag(_tile); return; }
        //Grass Foliage
        if (_tile.terrainType == TileTypes.TerrainType.GrassFoliage) { DropBag(_tile); return; }
        //Wood
        if (_tile.terrainType == TileTypes.TerrainType.Trunk) { DropBag(_tile, ItemList.ItemType.Log, 1, true); isTrunk(_tile); return; }
        //Leave
        if (_tile.terrainType == TileTypes.TerrainType.Leave) { DropBag(_tile); isLeaf(_tile); return; }
        //Stone
        if (_tile.terrainType == TileTypes.TerrainType.Stone) { DropBag(_tile, ItemList.ItemType.Stone, 1, false, true); isSnow(_tile); return; }
        //Dirt
        if (_tile.terrainType == TileTypes.TerrainType.Dirt) { DropBag(_tile, ItemList.ItemType.Dirt, 1, false, true);  isDirt(_tile); return; }
        //Grass
        if (_tile.terrainType == TileTypes.TerrainType.Grass) { DropBag(_tile, ItemList.ItemType.Dirt, 1, true); isGrass(_tile);  return; }
        //Snow
        if (_tile.terrainType == TileTypes.TerrainType.Snow) { DropBag(_tile, ItemList.ItemType.None, 0, false, true); return; }
        //Sand
        if (_tile.terrainType == TileTypes.TerrainType.Sand) { DropBag(_tile, ItemList.ItemType.Sand, 1, true); isSand(_tile); return; }
        //Sandstone
        if (_tile.terrainType == TileTypes.TerrainType.Sandstone) { DropBag(_tile, ItemList.ItemType.None, 0, false, true); return; }
        //Brambel
        if (_tile.terrainType == TileTypes.TerrainType.Bramble) { DropBag(_tile, ItemList.ItemType.BrambelVine, Random.Range(0,5), true); return; }
    }
    
    void DropBag(Tile _tile, ItemList.ItemType itemType = ItemList.ItemType.None, int itemAmount = 0, bool destroyTile = true, bool setBackground = false)
    {
        if (itemType == ItemList.ItemType.None)
        {
            if (destroyTile) { SetOtherTiles(_tile); _tile.DelayedSetBlockSides(); _tile.DestroyThisTile(); }
            if (setBackground){ _tile.ChangeToBackground();}
            return;
        }
        else
        {
            GameObject instBag = Instantiate(_tile.genChunkScript.tilePrefabs.droppedBagPrefab, new Vector3(_tile.gameObject.transform.position.x, _tile.gameObject.transform.position.y, _tile.gameObject.transform.position.z), Quaternion.identity);
            instBag.GetComponent<ItemDropped>().ItemAmount = itemAmount;
            instBag.GetComponent<ItemDropped>().itemType = itemType;
            if (destroyTile) { SetOtherTiles(_tile); _tile.DelayedSetBlockSides(); _tile.DestroyThisTile(); }
            if (setBackground) { _tile.ChangeToBackground(); }
        }
    }

    void SetOtherTiles(Tile _tile)
    {
        if(_tile.isupTileNull == false)
        {
            _tile.upTile.isdownTileNull = true;
        }
        if (_tile.isleftTileNull == false)
        {
            _tile.leftTile.isrightTileNull = true;
        }
        if (_tile.isrightTileNull == false)
        {
            _tile.rightTile.isleftTileNull = true;
        }
        if (_tile.isdownTileNull == false)
        {
            _tile.downTile.isupTileNull = true;
        }
    }

    void isTrunk(Tile _tile)
    {
        if(_tile.isleftTileNull == false)
        {
            if(_tile.leftTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.leftTile.BlockMined();
            }
        }

        if (_tile.isrightTileNull == false)
        {
            if (_tile.rightTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.rightTile.BlockMined();
            }
        }
        if (_tile.isupTileNull == false)
        {
            if (_tile.upTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.upTile.BlockMined();
            }
            if (_tile.upTile.terrainType == TileTypes.TerrainType.Trunk)
            {
                _tile.upTile.BlockMined();
            }
        }
    }

    void isLeaf(Tile _tile)
    {
        if (_tile.isleftTileNull == false)
        {
            if (_tile.leftTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.leftTile.BlockMined();
            }
        }

        if (_tile.isrightTileNull == false)
        {
            if (_tile.rightTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.rightTile.BlockMined();
            }
        }
        if (_tile.isupTileNull == false)
        {
            if (_tile.upTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.upTile.BlockMined();
            }
        }
        if (_tile.isdownTileNull == false)
        {
            if (_tile.downTile.terrainType == TileTypes.TerrainType.Leave)
            {
                _tile.downTile.BlockMined();
            }
        }
    }

    void isGrass(Tile _tile)
    {
        if(_tile.isupTileNull == false)
        {
            if(_tile.upTile.terrainType == TileTypes.TerrainType.Trunk || _tile.upTile.terrainType == TileTypes.TerrainType.Bramble || _tile.upTile.terrainType == TileTypes.TerrainType.Flower || _tile.upTile.terrainType == TileTypes.TerrainType.GrassFoliage)
            {
                _tile.upTile.BlockMined();
            }
        }
    }

    void isSand(Tile _tile)
    {
        if (_tile.isupTileNull == false)
        {
            if (_tile.upTile.terrainType == TileTypes.TerrainType.Trunk || _tile.upTile.terrainType == TileTypes.TerrainType.Bramble || _tile.upTile.terrainType == TileTypes.TerrainType.DeadBush || _tile.upTile.terrainType == TileTypes.TerrainType.GrassFoliage)
            {
                _tile.upTile.BlockMined();
            }
        }
    }

    void isSnow(Tile _tile)
    {
        if (_tile.isupTileNull == false)
        {
            if (_tile.upTile.terrainType == TileTypes.TerrainType.Trunk || _tile.upTile.terrainType == TileTypes.TerrainType.Bramble || _tile.upTile.terrainType == TileTypes.TerrainType.Flower || _tile.upTile.terrainType == TileTypes.TerrainType.GrassFoliage)
            {
                _tile.upTile.BlockMined();
            }
        }
    }

    void isDirt(Tile _tile)
    {
        if (_tile.isupTileNull == false)
        {
            if (_tile.upTile.terrainType == TileTypes.TerrainType.Trunk || _tile.upTile.terrainType == TileTypes.TerrainType.Bramble || _tile.upTile.terrainType == TileTypes.TerrainType.Flower || _tile.upTile.terrainType == TileTypes.TerrainType.GrassFoliage)
            {
                _tile.upTile.BlockMined();
            }
        }
    }
}
