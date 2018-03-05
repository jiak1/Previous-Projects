using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();

	}

    public void Wave()
    {
        anim.SetTrigger("waving");
    }

    public void Walk(bool walking)
    {
        if (walking)
        {
            anim.SetBool("walking", true);
        }else
        {
            anim.SetBool("walking", false);
        }
    }

    public void Run(bool running)
    {
        if (running)
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
    }
   
    public void Fly(bool flying)
    {
        if (flying)
        {
            anim.SetBool("flying", true);
        }
        else
        {
            anim.SetBool("flying", false);
        }
    }
}
