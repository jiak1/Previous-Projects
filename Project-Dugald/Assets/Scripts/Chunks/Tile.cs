using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public GenerateChunk genChunkScript;
    public TileTypes.BiomeType biomeType;
    public TileTypes.TerrainType terrainType;
    public int x;
    public int y;
    public int chunkID;
    //Needs to be a multiple of 8
    public int hardness = 64;
    public int brokenAmount = 0;

    public int GlobalTileX;
    public int GlobalTileY;

    public int transformX;
    public int transformY;

    public Tile leftTile;
    public Tile rightTile;
    public Tile upTile;
    public Tile downTile;

    public Material backgroundMat;

    public int flowerInt;

    //Tree Stuff
    public bool isTrunk = false;
    public bool isLeave = false;
    public bool isSapling = false;
    public int saplingType;
    public int treeHeight = 10;
    public int trunkHeightFromBase;

    public bool isBackground = false;

    public bool manMade = false;


    public bool isleftTileNull = false;
    public bool isrightTileNull = false;
    public bool isupTileNull = false;
    public bool isdownTileNull = false;

    public bool isCloud = false;

    public void ChangeToBackground()
    {

        isBackground = true;
        if (this == null)
        {
            return;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MeshRenderer>().material = backgroundMat;
        }
    }

    public void BlockMined()
    {
        UpdateTileSides();
        genChunkScript.chunkManager.gameObject.GetComponent<DestroyTileScript>().StartDestroyingTile(this);
    }

    public void TurnIntoCave()
    {
        if (this == null || isBackground == true)
        {
            return;
        }

        ChangeToBackground();

        int rnd = Random.Range(0, 10);
        if (rnd >= 2)
        {
            GenLeft();
            GenRight();
            GenUp();
            GenDown();
        }
    }

    void GenLeft()
    {

        if (isleftTileNull == true || leftTile == null)
        {
            return;
        }
        else
        {

            if (leftTile.terrainType == TileTypes.TerrainType.Water)
            {
                return;
            }
            else
            {

                int rnd = Random.Range(0, 100);

                if (rnd >= 30)
                {
                    leftTile.TurnIntoCave();
                }
            }
        }
    }

    void GenRight()
    {
        if (isrightTileNull == true || rightTile == null)
        {
            return;
        }
        else
        {
            if (leftTile.terrainType == TileTypes.TerrainType.Water)
            {
                return;
            }
            else
            {
                int rnd = Random.Range(0, 100);

                if (rnd >= 30)
                {
                    rightTile.TurnIntoCave();
                }
            }
        }
    }

    void GenDown()
    {
        if (isdownTileNull == true || downTile == null)
        {
            return;
        }
        else
        {
            if (leftTile.terrainType == TileTypes.TerrainType.Water)
            {
                return;
            }
            else
            {
                int rnd = Random.Range(0, 100);

                if (rnd >= 25)
                {
                    downTile.TurnIntoCave();
                }
            }
        }
    }

    void GenUp()
    {
        if (isupTileNull == true || upTile == null)
        {
            return;
        }
        else
        {
            if (leftTile.terrainType == TileTypes.TerrainType.Water)
            {
                return;
            }
            else
            {
                int rnd = Random.Range(0, 100);

                if (rnd >= 75)
                {
                    upTile.TurnIntoCave();
                }
            }
        }
    }

    public void UpdateTileSides()
    {
        
        Tile _tile1 = this;

        if(_tile1 == null)
        {
            return;
        }

        RaycastHit2D hit;
        Vector3 rayOrigin = _tile1.gameObject.transform.position;
        hit = Physics2D.Raycast(rayOrigin, Vector3.left, 1);

        if (hit.collider == null || hit.collider.tag == "Player")
        {
            _tile1.leftTile = _tile1;
            _tile1.leftTile.isleftTileNull = true;
        }
        else
        {

            if (hit.transform.gameObject.GetComponent<Tile>() != null)
            {
                _tile1.leftTile = hit.collider.transform.gameObject.GetComponent<Tile>();
            }


        }





        RaycastHit2D hit2;
        Vector3 rayOrigin2 = _tile1.gameObject.transform.position;
        hit2 = Physics2D.Raycast(rayOrigin2, Vector3.right, 1);

        if (hit2.collider == null || hit2.collider.tag == "Player")
        {
            _tile1.rightTile = _tile1;
            _tile1.rightTile.isrightTileNull = true;
        }
        else
        {

            if (hit2.transform.gameObject.GetComponent<Tile>() != null)
            {
                _tile1.rightTile = hit2.collider.transform.gameObject.GetComponent<Tile>();
            }


        }





        RaycastHit2D hit3;
        Vector3 rayOrigin3 = _tile1.gameObject.transform.position;
        hit3 = Physics2D.Raycast(rayOrigin3, Vector3.up, 1);

        if (hit3.collider == null || hit3.collider.tag == "Player")
        {
            _tile1.upTile = _tile1;
            _tile1.upTile.isupTileNull = true;
        }
        else
        {

            if (hit3.transform.gameObject.GetComponent<Tile>() != null)
            {
                _tile1.upTile = hit3.collider.transform.gameObject.GetComponent<Tile>();
            }


        }






        RaycastHit2D hit4;
        Vector3 rayOrigin4 = _tile1.gameObject.transform.position;
        hit4 = Physics2D.Raycast(rayOrigin4, Vector3.down, 1);

        if (hit4.collider == null || hit4.collider.tag == "Player")
        {
            _tile1.downTile = _tile1;
            _tile1.downTile.isdownTileNull = true;
        }
        else
        {

            if (hit4.transform.gameObject.GetComponent<Tile>() != null)
            {
                _tile1.downTile = hit4.collider.transform.gameObject.GetComponent<Tile>();
            }


        }
    }

    public void DestroyThisTile()
    {
        genChunkScript.tiles.Remove(this);
        Destroy(this.gameObject);
    }

    public void UpdateSides()
    {
        UpdateTileSides();
    }

    public void updateOtherBlocks()
    {
        if (!isleftTileNull)
        {
            leftTile.UpdateSides();
        }
        if (!isrightTileNull)
        {
            rightTile.UpdateSides();
        }
        if (!isupTileNull)
        {
            upTile.UpdateSides();
        }
        if (!isdownTileNull)
        {
            downTile.UpdateSides();
        }
    }

    IEnumerator wait2()
    {

        yield return new WaitForSeconds(5);
        SetwaterMaterial();
    }

    IEnumerator wait5()
    {

        yield return new WaitForSeconds(4);
        updateOtherBlocks();
    }
    
    public void DelayedSetBlockSides()
    {
        UpdateTileSides();

        if (!isleftTileNull)
        {
            if(leftTile != null)
            {
                leftTile.StartCoroutine(wait5());
            }
            else
            {
                isleftTileNull = true;
            }
        }
        if (!isrightTileNull)
        {
            if(rightTile != null)
            {
                rightTile.StartCoroutine(wait5());
            }
            else
            {
                isrightTileNull = true;
            }
        }
        if (!isupTileNull)
        {
            if(upTile != null)
            {
                upTile.StartCoroutine(wait5());
            }
            else
            {
                isupTileNull = true;
            }
        }
        if (!isdownTileNull)
        {
            if(downTile != null)
            {
                downTile.StartCoroutine(wait5());
            }else
            {
                isdownTileNull = true;
            }
        }
    }

    public void SetwaterMaterial()
    {

        if (terrainType == TileTypes.TerrainType.Water)
        {
            if (isupTileNull && isleftTileNull)
            {
                GetComponent<MeshRenderer>().material = genChunkScript.tilePrefabs.waterSlope1Mat;
            }
            if (isupTileNull && isrightTileNull)
            {
                GetComponent<MeshRenderer>().material = genChunkScript.tilePrefabs.waterSlope2Mat;
            }
        }


    }



    private void Awake()
    {
        if (terrainType == TileTypes.TerrainType.Water)
        {
            UpdateTileSides();
        }

    }

    void UnAnnoyJack()
    {
        UpdateTileSides();

        if (isupTileNull == false)
        {
            upTile.isdownTileNull = false;
            upTile.downTile = this;
        }
        if (isleftTileNull == false)
        {
            leftTile.isrightTileNull = false;
            leftTile.rightTile = this;
        }
        if (isrightTileNull == false)
        {
            rightTile.isleftTileNull = false;
            rightTile.leftTile = this;
        }
        if (isdownTileNull == false)
        {
            downTile.isupTileNull = false;
            downTile.upTile = this;
        }
    }

    private void Start()
    {
        if(terrainType == TileTypes.TerrainType.Error)
        {
            backgroundMat = genChunkScript.tilePrefabs.errorMat;
        }
        if (backgroundMat == null)
        {
            backgroundMat = genChunkScript.tilePrefabs.stoneBackgroundMat;
        }
        transformX = x - 1;
        transformY = y - 1;
        if (isSapling)
        {
            for (int i = 0; i < treeHeight; i++)
            {

                GameObject newTrunk = genChunkScript.SpawnTile2(transformX, transformY + i, genChunkScript.tilePrefabs.trunkTile[saplingType]);
                Tile _t = newTrunk.GetComponent<Tile>();
                _t.saplingType = saplingType;
                _t.terrainType = TileTypes.TerrainType.Trunk;
                _t.trunkHeightFromBase = i + 1;
                _t.treeHeight = treeHeight;
                _t.isTrunk = true;

            }
            genChunkScript.tiles.Remove(this);
            Destroy(this.gameObject);
        }

        if (isTrunk)
        {
            if (trunkHeightFromBase == 1)
            {

                if (saplingType == 0)
                {
                    GameObject newTrunk = genChunkScript.SpawnTile2(transformX, transformY, genChunkScript.tilePrefabs.dirtTile);
                    Tile _t = newTrunk.GetComponent<Tile>();
                    _t.terrainType = TileTypes.TerrainType.Dirt;
                    _t.UnAnnoyJack();
                }
                if (saplingType == 1)
                {
                    GameObject newTrunk = genChunkScript.SpawnTile2(transformX, transformY, genChunkScript.tilePrefabs.snowTile);
                    Tile _t = newTrunk.GetComponent<Tile>();
                    _t.terrainType = TileTypes.TerrainType.Snow;
                    _t.UnAnnoyJack();
                }
                DestroyThisTile();
            }
            GetComponent<BoxCollider2D>().isTrigger = true;
            UpdateTileSides();
            if (!isleftTileNull)
            {
                leftTile.UpdateTileSides();
            }
            if (!isrightTileNull)
            {
                rightTile.UpdateTileSides();
            }
            if (!isupTileNull)
            {
                upTile.UpdateTileSides();
            }
            if (!isdownTileNull)
            {
                downTile.UpdateTileSides();
            }

            int y = treeHeight / 2;
            if (trunkHeightFromBase > y)
            {
                if (isupTileNull)
                {
                    GameObject newLeave = genChunkScript.SpawnTile2(transformX, transformY + 1, genChunkScript.tilePrefabs.leaveTile[saplingType]);
                    newLeave.GetComponent<Tile>().isLeave = true;
                    newLeave.transform.position = new Vector3(newLeave.transform.position.x, newLeave.transform.position.y, -1);
                    isupTileNull = false;
                    upTile = newLeave.GetComponent<Tile>();
                }
                if (isleftTileNull)
                {
                    GameObject newLeave = genChunkScript.SpawnTile2(transformX - 1, transformY, genChunkScript.tilePrefabs.leaveTile[saplingType]);
                    newLeave.GetComponent<Tile>().isLeave = true;
                    newLeave.transform.position = new Vector3(newLeave.transform.position.x, newLeave.transform.position.y, -1);
                    leftTile = newLeave.GetComponent<Tile>();
                    isleftTileNull = false;
                }
                if (isrightTileNull)
                {
                    GameObject newLeave = genChunkScript.SpawnTile2(transformX + 1, transformY + 1, genChunkScript.tilePrefabs.leaveTile[saplingType]);
                    newLeave.GetComponent<Tile>().isLeave = true;
                    newLeave.transform.position = new Vector3(newLeave.transform.position.x, newLeave.transform.position.y, -1);
                    rightTile = newLeave.GetComponent<Tile>();
                    isrightTileNull = false;
                }

                if (!isdownTileNull)
                {
                    if (downTile.terrainType == TileTypes.TerrainType.Grass && saplingType == 0)
                    {
                        downTile.DestroyThisTile();
                        GameObject downT = genChunkScript.SpawnTile2(transformX, transformY - 1, genChunkScript.tilePrefabs.dirtTile);
                        downTile = downT.GetComponent<Tile>();
                        downTile.UnAnnoyJack();
                    }
                }
                UpdateTileSides();
            }
        }

        if (isLeave)
        {
            terrainType = TileTypes.TerrainType.Leave;
            GetComponent<BoxCollider2D>().isTrigger = true;
            UpdateTileSides();
            if (!isleftTileNull)
            {
                leftTile.UpdateTileSides();
            }
            if (!isrightTileNull)
            {
                rightTile.UpdateTileSides();
            }
            if (!isupTileNull)
            {
                upTile.UpdateTileSides();
            }
            if (!isdownTileNull)
            {
                downTile.UpdateTileSides();
            }
        }

        if (terrainType == TileTypes.TerrainType.Water)
        {
            StartCoroutine(wait2());
        }

        if (isupTileNull && isdownTileNull && isleftTileNull && isrightTileNull)
        {
            if (terrainType == TileTypes.TerrainType.Dirt || terrainType == TileTypes.TerrainType.Grass)
            {
                DestroyThisTile();
            }
        }

        if (isCloud)
        {
            if (isleftTileNull && isdownTileNull && isupTileNull)
            {
                DestroyThisTile();
            }

            if (isrightTileNull && isdownTileNull && isupTileNull)
            {
                DestroyThisTile();
            }

            if (isupTileNull && isrightTileNull && isleftTileNull)
            {
                DestroyThisTile();
            }

            UpdateTileSides();
        }

        if (GetComponent<BoxCollider2D>().isTrigger)
        {
            UnAnnoyJack();
        }
        if (manMade)
        {
            UpdateTileSides();
            updateOtherBlocks();
        }
    }
}
