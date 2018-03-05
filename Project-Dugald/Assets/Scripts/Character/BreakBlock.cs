using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    Tile previousTile;
    GameObject previousBreakLines;
    public GameObject prefabBreakLines;
    public Sprite[] breakLinesSprites;
    public int maxBreakDistance = 10;
    //4 Sprites

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 5f;

            Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D[] col = Physics2D.OverlapPointAll(v);

            if (col.Length > 0)
            {
                foreach (Collider2D c in col)
                {
                    //Debug.Log("Collided with: " + c.gameObject.name);

                    GameObject hit = c.gameObject;


                    //if (hit)
                    //{
                    //    Debug.Log(hit.transform.name);
                    //}
                    Transform objectHit = hit.transform;

                    if (objectHit.GetComponent<Tile>() == null)
                    {
                        //Debug.Log("1");
                        return;
                    }
                    else
                    {
                        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>() != null)
                        {
                            if (objectHit.GetComponent<Tile>().isBackground)
                            {
                                //Debug.Log("2");
                                return;
                            }
                            else
                            {
                                if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, objectHit.transform.position) < maxBreakDistance)
                                {
                                    if (previousTile != objectHit.GetComponent<Tile>() && previousTile != null)
                                    {
                                        previousTile.brokenAmount = 0;
                                        Destroy(previousBreakLines);

                                        if (objectHit.GetComponent<Tile>().brokenAmount == objectHit.GetComponent<Tile>().hardness)
                                        {
                                            //Debug.Log("3");
                                            objectHit.GetComponent<Tile>().BlockMined();
                                            Destroy(previousBreakLines);
                                        }
                                        else
                                        {
                                            Destroy(previousBreakLines);
                                            //Debug.Log("4");
                                            objectHit.GetComponent<Tile>().brokenAmount = objectHit.GetComponent<Tile>().brokenAmount + 1;
                                            previousBreakLines = Instantiate(prefabBreakLines, new Vector3(objectHit.transform.position.x, objectHit.transform.position.y, objectHit.transform.position.z), Quaternion.identity);
                                            previousTile = objectHit.GetComponent<Tile>();



                                            previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[1];


                                            if ((objectHit.GetComponent<Tile>().hardness / 4) <= objectHit.GetComponent<Tile>().brokenAmount)
                                            {
                                                previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[2];
                                            }

                                            if ((objectHit.GetComponent<Tile>().hardness / 4) * 2 <= objectHit.GetComponent<Tile>().brokenAmount)
                                            {
                                                previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[3];
                                            }
                                            //if its over 3/4 broken
                                            if ((objectHit.GetComponent<Tile>().hardness / 4) * 3 <= objectHit.GetComponent<Tile>().brokenAmount)
                                            {
                                                previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[3];
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (objectHit.GetComponent<Tile>().brokenAmount == objectHit.GetComponent<Tile>().hardness)
                                        {
                                            //Debug.Log("3");
                                            objectHit.GetComponent<Tile>().BlockMined();
                                            Destroy(previousBreakLines);
                                        }
                                        else
                                        {
                                            Destroy(previousBreakLines);
                                            //Debug.Log("4");
                                            objectHit.GetComponent<Tile>().brokenAmount = objectHit.GetComponent<Tile>().brokenAmount + 1;
                                            previousBreakLines = Instantiate(prefabBreakLines, new Vector3(objectHit.transform.position.x, objectHit.transform.position.y, objectHit.transform.position.z), Quaternion.identity);
                                            previousTile = objectHit.GetComponent<Tile>();



                                            previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[1];


                                            if ((objectHit.GetComponent<Tile>().hardness / 4) <= objectHit.GetComponent<Tile>().brokenAmount)
                                            {
                                                previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[2];
                                            }

                                            if ((objectHit.GetComponent<Tile>().hardness / 4) * 2 <= objectHit.GetComponent<Tile>().brokenAmount)
                                            {
                                                previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[3];
                                            }
                                            //if its over 3/4 broken
                                            if ((objectHit.GetComponent<Tile>().hardness / 4) * 3 <= objectHit.GetComponent<Tile>().brokenAmount)
                                            {
                                                previousBreakLines.GetComponent<SpriteRenderer>().sprite = breakLinesSprites[3];
                                            }
                                        }
                                    }
                                }

                            }
                        }

                    }
                }
            }
        }
        else
        {
            if (previousTile != null)
            {
                previousTile.brokenAmount = 0;
                Destroy(previousBreakLines);
            }
        }
    }
}


