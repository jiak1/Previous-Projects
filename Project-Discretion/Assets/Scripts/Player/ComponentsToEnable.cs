using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsToEnable : MonoBehaviour {

    [SerializeField]
    public Camera cam;

    public bool runOnStartup = false;
    private void Start()
    {
        if (runOnStartup)
        {
            EnableComponents();
        }
    }
    
	public void EnableComponents () {
        cam.enabled = true;
        cam.gameObject.GetComponent<AudioListener>().enabled = true;
        GetComponent<PlayerAnimator>().enabled = true;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<PlayerNetworker>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
    }

}
