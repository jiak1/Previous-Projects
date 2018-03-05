using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplainOnStartup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("COMPLAING FROM " + gameObject.transform.parent.name);	
	}
	
}
