using UnityEngine;

public class MouseController : MonoBehaviour {
    [SerializeField]
    private GamesSettingsManager gs;

    private Camera cam;
    [SerializeField]
    private Transform TargetLookAt;

    
    public float Distance = 5.0f;
    public float DistanceMin = 3.0f;
    public float DistanceMax = 10.0f;
    public float mouseX = 0.0f;
    public float mouseY = 0.0f;
    public float startingDistance = 0.0f;
    public float desiredDistance = 0.0f;
    public float X_MouseSensitivity = 5.0f;
    public float Y_MouseSensitivity = 5.0f;
    public float MouseWheelSensitivity = 5.0f;
    public float Y_MinLimit = -40.0f;
    public float Y_MaxLimit = 80.0f;
    public float DistanceSmooth = 0.05f;
    public float velocityDistance = 0.0f;    
    private Vector3 desiredPosition = Vector3.zero;
    public float X_Smooth = 0.05f;
    public float Y_Smooth = 0.1f;
    public float velX = 0.0f;
    public float velY = 0.0f;
    public float velZ = 0.0f;
    private Vector3 position = Vector3.zero;    

	// Use this for initialization
	void Start () {
        cam = Camera.main;

        Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
        startingDistance = Distance;
        Reset();
	}

    void LateUpdate()
    {
        if (gs.getIsPaused() == true)
        {
            return;
        }else
        {
            if (TargetLookAt == null)
            {
                return;
            }
            else
            {
                HandlePlayerInput();
                CalculateDesiredPosition();
                UpdatePosition();
            }
        }

    }


    void HandlePlayerInput()
    {
        float deadZone = 0.01f; // Mousewheel deadZone

        if (Input.GetMouseButton(2))
        {
            mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
            mouseY += Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
        }

        //This is where the mouseY is limited
        mouseY = ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

        //GetComponent Mouse Wheel Input
        if(Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(Distance - (Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity), DistanceMin, DistanceMax);

        }
    }

    void CalculateDesiredPosition()
    {
        //Evaluate Distance
        Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velocityDistance, DistanceSmooth);

        //Calculate desired position > Note: mouse inputs reversed to align to worldpsace axis
        desiredPosition = CalculatePosition(-mouseY, mouseX, Distance);
    }

   Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        return TargetLookAt.position + (rotation * direction);
    }

    void UpdatePosition()
    {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_Smooth);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_Smooth);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_Smooth);

        position = new Vector3(posX, posY, posZ);

        cam.transform.position = position;

        cam.transform.LookAt(TargetLookAt);
    }

    private void Reset()
    {

        mouseX = 0;
        mouseY = 10;
        Distance = startingDistance;
        desiredDistance = Distance;

    }

    float ClampAngle(float angle, float min, float max)
    {

        while (angle < -360 || angle > 360)
        {
            if (angle < -360)
            {
                angle += 360;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
        }
        return Mathf.Clamp(angle, min, max);


    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Selecting

        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            //Panning Around Center
            
            
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            //Panning Around Map

        }
    }
}
