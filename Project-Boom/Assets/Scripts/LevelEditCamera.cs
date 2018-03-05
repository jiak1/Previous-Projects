using UnityEngine;
using UnityEngine.EventSystems;
public class LevelEditCamera : MonoBehaviour
{

    float panSpeed = 20f;
    float panBorderThickness = 5f;
    public Vector2 bounds;

    LevelEditor lvE;
    float scrollDampening = 10f;
    float orbitDampening = 6f;
    float scrollSensitivity = 2f;
    float mouseSensitivity = 4f;
    float middleMouseMoveDampener = 2f;
    float minScroll = 20f;
    float maxScroll = 60f;
    EventSystem ev;
    Vector3 movePos = Vector3.zero;
    float lastScroll = 0f;
    Transform xFormCamera;
    Transform xFormParent;
    bool dothis = false;
    Vector3 localRotation;
    float cameraDistance = 10f;
    float panSensitivity = 0.75f;
    Vector3 lastMousePos;
    enum mouseMode { Nothing,Panning,Orbiting,Zooming }
    mouseMode mMode;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        mMode = mouseMode.Nothing;
        lvE = GameObject.FindObjectOfType<LevelEditor>();
        ev = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        xFormCamera = this.transform;
        xFormParent = this.transform.parent;
    }

    private void Update()
    {
        //ORBITING
        if (Input.GetKeyDown(KeyCode.Mouse2) && Input.GetKey(KeyCode.LeftShift) == false && Input.GetKey(KeyCode.RightShift) == false)
        {
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {
                movePos = _hit.point;
            }
        }
        if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.LeftShift) == false && Input.GetKey(KeyCode.RightShift) == false )
        {
            xFormParent.transform.position = Vector3.Lerp(xFormParent.transform.position, movePos, Time.deltaTime * middleMouseMoveDampener);

        }
        //PANNING
        if (Input.GetKeyDown(KeyCode.LeftShift)  || Input.GetKeyDown(KeyCode.RightShift)|| Input.GetKeyDown(KeyCode.Mouse2) )
        {

            lastMousePos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.RightShift))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.Translate(-delta.x *panSensitivity*Time.deltaTime,-delta.y *panSensitivity * Time.deltaTime, 0);
            lastMousePos = Input.mousePosition;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (lvE == null) { lvE = GameObject.FindObjectOfType<LevelEditor>(); }
        if (lvE.blockSelected == false && lvE.inEditor && ev.IsPointerOverGameObject() == false)
        {
            //ORBITING
            if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.LeftShift) == false && Input.GetKey(KeyCode.RightShift) == false )
            {
                //Rotate camera based on mouse coordinates
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                    localRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                    //Clamp values to horizon and not flipping
                    localRotation.y = Mathf.Clamp(localRotation.y, 1f, 90f);
                    dothis = true;
                }
            }
            //Zooming
            if (Input.GetAxis("Mouse ScrollWheel") != lastScroll)
            {
                mMode = mouseMode.Zooming;
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
                lastScroll = scrollAmount;
                scrollAmount *= cameraDistance * 0.3f;

                cameraDistance += scrollAmount * -1f;

                cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 1000f);
                dothis = true;
            }

            if (dothis)
            {
                //Camera transformations
                Quaternion qt = Quaternion.Euler(localRotation.y, localRotation.x, 0);

                xFormParent.rotation = Quaternion.Lerp(xFormParent.rotation, qt, Time.deltaTime * orbitDampening);

                if (xFormCamera.localPosition.z != cameraDistance * -1f)
                {
                    xFormCamera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(xFormCamera.localPosition.z, cameraDistance * -1f, Time.deltaTime * scrollDampening));
                }
                dothis = false;
            }
            //Vector3 pos = transform.position;

            //if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= (Screen.height - panBorderThickness))
            //{
            //    pos.z += panSpeed * Time.deltaTime;
            //}
            //if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
            //{
            //    pos.z -= panSpeed * Time.deltaTime;
            //}
            //if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= (Screen.width - panBorderThickness))
            //{
            //    pos.x += panSpeed * Time.deltaTime;
            //}
            //if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
            //{
            //    pos.x -= panSpeed * Time.deltaTime;
            //}

            //float scroll = Input.GetAxis("Mouse ScrollWheel");
            //pos.y += -scroll * scrollSpeed * 100 * Time.deltaTime;

            //pos.y = Mathf.Clamp(pos.y, minScroll, maxScroll);

            //pos.x = Mathf.Clamp(pos.x, -bounds.x, bounds.x);
            //pos.z = Mathf.Clamp(pos.z, -bounds.y, bounds.y);

            //transform.position = pos;


        }

    }
}
