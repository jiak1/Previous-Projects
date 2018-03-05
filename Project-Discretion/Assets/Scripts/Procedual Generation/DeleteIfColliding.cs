using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIfColliding : MonoBehaviour {

    //[ExecuteInEditMode]
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Running");
        if (collision.collider.CompareTag("Tree"))
        {
            if (Application.isEditor)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        Debug.Log("Running2");
        if (collision.collider.CompareTag("Tree"))
        {
            if (Application.isEditor)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
