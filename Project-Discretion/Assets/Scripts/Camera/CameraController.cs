using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jack;
public class CameraController : MonoBehaviour {

    public Vector2 panLimit;

    private void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - GameVars.panBorderThickness)
        {
            pos.z += GameVars.panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= GameVars.panBorderThickness)
        {
            pos.z -= GameVars.panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= GameVars.panBorderThickness)
        {
            pos.x -= GameVars.panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - GameVars.panBorderThickness)
        {
            pos.x += GameVars.panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y += scroll * GameVars.scrollSpeed * 100 * Time.deltaTime;


        pos.y = Mathf.Clamp(pos.y, GameVars.scrollmin, GameVars.scrollmax);
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
