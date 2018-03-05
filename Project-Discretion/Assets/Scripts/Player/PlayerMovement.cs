using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;
    public float sprintSpeedMultiplier = 2.5f;
    public float lookSensitivity = 3f;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<ComponentsToEnable>().cam;
    }

    void Update()
    {
        //Get movement as Vector3
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        //Move left and right
        Vector3 _moveHorizontal = transform.right * _xMov;
        Vector3 _moveVertical = transform.forward * _zMov;
        Vector3 _velocity;

        //Final Movement Vector
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _velocity = (_moveHorizontal + _moveVertical).normalized * speed * sprintSpeedMultiplier;
        }else
        {
            _velocity = (_moveHorizontal + _moveVertical).normalized * speed;
        }

        //Apply movement
        Move(_velocity);


        //Get left and right look rotations for up and down we move camera
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        Rotate(_rotation);

        //Get up and down rotations and move camera
        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        RotateCamera(_cameraRotation);
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        FixRotations();
    }

    void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    void FixRotations()
    {
        gameObject.transform.rotation.z.Equals(0);
        gameObject.transform.rotation.x.Equals(0);
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
        cam.transform.Rotate(-cameraRotation);
    }
}
