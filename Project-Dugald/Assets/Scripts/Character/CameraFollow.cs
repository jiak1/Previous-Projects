using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

    public GameObject target;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);
        }
    }

    void Update()
    {
        float fov = Camera.main.orthographicSize;
        fov += -(Input.GetAxis("Mouse ScrollWheel")) * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.orthographicSize = fov;
    }
}
