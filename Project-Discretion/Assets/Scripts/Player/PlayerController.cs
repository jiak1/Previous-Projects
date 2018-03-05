using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAnimator playerAnimator;
    private PlayerMovement playerMovement;
    public MonoBehaviour flyCam;
    private bool isFlying = true;

    // Use this for initialization
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
        Fly(isFlying);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerAnimator.Wave();
            GetComponent<PlayerNetworker>().InitiateWave();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isFlying) { isFlying = false; transform.rotation.Equals(Vector3.zero); } else if (!(isFlying)) { isFlying = true; }
            Fly(isFlying);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (flyCam.enabled)
            {
                flyCam.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                flyCam.enabled = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }

        }
    }

    void Fly(bool flying)
    {
        if (flying)
        {
            playerMovement.enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            flyCam.enabled = true;
        }
        else
        {
            playerMovement.enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            flyCam.enabled = false;
        }

        playerAnimator.Fly(flying);
    }
}
