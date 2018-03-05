using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    public float sprintSpeed;
    public float speed;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpSpeed;
    public GameObject mainVisual;
    bool lookingRight;
    bool isWalking;
    bool isGrounded;

    public ArmRotation armRot;

    public SpriteRenderer facialHairRnd;
    public SpriteRenderer hairRnd;

    public List<Sprite> hair = new List<Sprite>();
    public List<Sprite> facialHair = new List<Sprite>();

    public float groundCheckDistance = 1.0f;

    RaycastHit2D hit;

    private void Start()
    {
        int rnd = Random.Range(0, facialHair.Count - 1);
        facialHairRnd.sprite = facialHair[rnd];
        int rnd2 = Random.Range(0, hair.Count - 1);
        hairRnd.sprite = hair[rnd2];


        lookingRight = true;
    }

    void FixedUpdate()
    {
        if (lookingRight)
        {
            armRot.rotationOffset = 0;
        }
        else
        {
            armRot.rotationOffset = -180;
        }
        float movex = Input.GetAxis("Horizontal");
        CheckIsGrounded();

        Rigidbody2D r = GetComponent<Rigidbody2D>();

        if ((movex > 0 && !lookingRight) || (movex < 0 && lookingRight))
        {
            Flip();
        }

        if (Input.GetKeyDown(jumpKey))
        {
            if (isGrounded)
            {
                r.velocity = new Vector2((movex * speed) / 2, speed * jumpSpeed);
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                r.velocity = new Vector2(movex * sprintSpeed, r.velocity.y);
            }
            else
            {

                r.velocity = new Vector2(movex * speed, r.velocity.y);

            }



        }
        else
        {
            r.AddForce(new Vector3(0, 0, 0));
        }



        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) || isGrounded == false)
        {
            r.AddForce(new Vector3(0, 0, 0));
        }

    }
    public void CheckIsGrounded()
    {
        float distToGround;
        distToGround = groundCheckDistance;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        if (hit.collider == null)
        {
            isGrounded = false;
        }
        else
        {
            if (hit.collider.GetComponent<Tile>() != null)
            {
                if (hit.collider.isTrigger == false)
                {
                    isGrounded = true;
                }
                else
                {
                    if (hit.collider.GetComponent<Tile>().terrainType == TileTypes.TerrainType.Water)
                    {
                        if (hit.collider.GetComponent<Tile>().isdownTileNull)
                        {
                            isGrounded = false;
                        }
                        else
                        {
                            if (hit.collider.GetComponent<Tile>().downTile.terrainType == TileTypes.TerrainType.Water)
                            {
                                isGrounded = false;
                            }
                            else
                            {
                                isGrounded = true;
                            }
                        }
                    }
                    if (hit.collider.GetComponent<Tile>().isdownTileNull)
                    {
                        isGrounded = false;
                    }
                    else
                    {
                        if (hit.collider.GetComponent<Tile>().downTile.terrainType == TileTypes.TerrainType.Dirt || hit.collider.GetComponent<Tile>().downTile.terrainType == TileTypes.TerrainType.Grass || hit.collider.GetComponent<Tile>().downTile.terrainType == TileTypes.TerrainType.Snow || hit.collider.GetComponent<Tile>().downTile.terrainType == TileTypes.TerrainType.Sand)
                        {
                            isGrounded = true;
                        }
                        else
                        {
                            isGrounded = false;
                        }
                    }

                }
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    public void Flip()
    {
        lookingRight = !lookingRight;
        Vector3 myScale = mainVisual.transform.localScale;
        myScale.x *= -1;
        mainVisual.transform.localScale = myScale;
    }


}
