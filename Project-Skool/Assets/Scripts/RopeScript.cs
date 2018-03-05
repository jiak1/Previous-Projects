using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

    public float RopeStartSpeed = 1.0f;
    private HingeJoint2D[] jointsToStart;
    // Use this for initialization
    void Start () {

        jointsToStart = GetComponentsInChildren<HingeJoint2D>();
        foreach (HingeJoint2D joint in jointsToStart)
        {
            Vector2 force = new Vector2(1*RopeStartSpeed, 0);
            joint.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
	}
	
}
