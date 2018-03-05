using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHeights : MonoBehaviour {
    Vector3 appropriateSpot;
	// Use this for initialization
	void Start () {
        appropriateSpot = new Vector3(transform.localPosition.x, -44f, transform.localPosition.z);
        if(transform.position.y != -44f) { transform.localPosition = appropriateSpot; }
	}
}
