using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimb : MonoBehaviour
{

    public SortingLayer limbLayer;

    public GameObject leftArmPoint;
    public TargetJoint2D leftArmTarget;

    public GameObject rightArmPoint;
    public TargetJoint2D rightArmTarget;

    public GameObject leftLegPoint;
    public TargetJoint2D leftLegTarget;

    public GameObject rightLegPoint;
    public TargetJoint2D rightLegTarget;

    private List<TargetJoint2D> jointsToDisable = new List<TargetJoint2D>();

    public RaycastHit2D hit;
    public bool mouseDown = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TurnBackOffJoints();
        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseDown == false)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
         //   Debug.Log(hit.transform.gameObject.layer.ToString());
            if(hit.collider.tag == "Limb")
            {
              //  Debug.Log("First Time");
                //First Time selecting joint
                mouseDown = true;
            }else
            {
               // Debug.Log("Did not hit limb instead hit: " + hit.collider.name);
                return;
            }
            

        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0) && mouseDown == true)
            {

               // Debug.Log("Dragging");
                //Dragging
                Dragging();
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    ResetLimbJoints();
                    mouseDown = false;
                }
            }
        }



    }

    void TurnBackOffJoints()
    {
        for (int i = 0; i < jointsToDisable.Count; i++)
        {
            jointsToDisable[i].enabled = false;
            jointsToDisable.RemoveAt(i);
        }
    }

    void ResetLimbJoints()
    {
        leftArmTarget.target = leftArmPoint.transform.position;
        rightArmTarget.target = rightArmPoint.transform.position;
        leftLegTarget.target = leftLegPoint.transform.position;
        rightLegTarget.target = rightLegPoint.transform.position;

    }

    void Dragging()
    {

        if (hit.collider == null)
        {
            return;
        }

       // Debug.Log("Hit " + hit.transform.name.ToString());
        if (hit.collider.name.Contains(leftArmPoint.transform.name))
        {
            //Left Arm Point Hit
            leftArmTarget.enabled = true;
            RaycastHit2D hit1 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            leftArmTarget.target = hit1.point;
          //  Debug.Log("Set LA Target");
            jointsToDisable.Add(leftArmTarget);
           leftArmPoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            if (hit.collider.name.Contains(rightArmPoint.transform.name))
            {
                //Right Arm Point Hit
                rightArmTarget.enabled = true;
                RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                rightArmTarget.target = hit2.point;
               // Debug.Log("Set RA Target");
                jointsToDisable.Add(rightArmTarget);
                rightArmPoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                if (hit.collider.name.Contains(leftLegPoint.transform.name))
                {
                    //Left Leg Point Hit
                    leftLegTarget.enabled = true;
                    RaycastHit2D hit3 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    leftLegTarget.target = hit3.point;
                   // Debug.Log("Set LL Target");
                    jointsToDisable.Add(leftLegTarget);
                    leftLegPoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                }
                else
                {
                    if (hit.collider.name.Contains(rightLegPoint.transform.name))
                    {
                        //Right Leg Point Hit
                        rightLegTarget.enabled = true;
                        RaycastHit2D hit4 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        rightLegTarget.target = hit4.point;
                        //Debug.Log("Set RL Target");
                        jointsToDisable.Add(rightLegTarget);
                        rightLegPoint.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    }
                }
            }
        }

    }
}
