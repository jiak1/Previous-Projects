using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGrab : MonoBehaviour {

    public TargetJoint2D targetJoint;
    public Rigidbody2D targetRB2D;

    public void Start()
    {
        targetJoint.enabled = false;
        targetJoint.target = this.transform.position;
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Grabbable"))
        {
          //  Debug.Log("Grabbable");
            targetJoint.enabled = true;
            targetJoint.target = this.transform.position;
            targetRB2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            //Debug.Log("Nope Collider Had Tag: " + collision.tag);

            return;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grabbable"))
        {
           // Debug.Log("Grabbable");
            targetJoint.enabled = false;
            targetRB2D.constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            //Debug.Log("Nope Collider Had Tag: " + collision.tag);
            return;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Grabbable"))
        {
          //  Debug.Log("Grabbable");
            targetJoint.enabled = true;
            targetRB2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            //Debug.Log("Nope Collider Had Tag: " + collision.tag);
            
            return;
        }
    }
}
