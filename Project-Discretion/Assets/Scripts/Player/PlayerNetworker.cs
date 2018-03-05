using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworker : MonoBehaviour {

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    public Animator anim;
    private PhotonView pView;
    public bool ourPlayer = false;

    bool needToWave = false;
	// Use this for initialization
	void Start () {
        pView = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!(pView.isMine) && !(ourPlayer))
        {
            //This isn't our player smooth their movement
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
            Debug.Log("Ts");
        }
	}

    void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //This is our player we need to send our position to the network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetBool("flying"));
            stream.SendNext(anim.GetBool("walking"));
            stream.SendNext(needToWave);
            if (needToWave) { needToWave = false; }

        }
        else
        {
            Debug.Log("T");
            //This is another persons player we need to recieve and update our version of that player
            //transform.position = (Vector3) stream.ReceiveNext();
            //transform.rotation = (Quaternion)stream.ReceiveNext();

            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();

            anim.SetBool("flying", (bool)stream.ReceiveNext());
            anim.SetBool("walking", (bool)stream.ReceiveNext());
            bool startWave = (bool)stream.ReceiveNext();
            if (startWave) { anim.SetTrigger("waving"); }
        }
    }


    public void InitiateWave()
    {
        needToWave = true;
    }
}
