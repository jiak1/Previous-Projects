using UnityEngine;
using UnityEngine.UI;
public class SelectLocationOnSphere : MonoBehaviour
{
    [SerializeField]
    private GamesSettingsManager gs;

    private RaycastHit hit;
    [SerializeField]
    private GameObject marker;

    private RectTransform markerT;
    private Image markerI;

    [SerializeField]
    private Sprite[] markerImages;
    [SerializeField]
    private float markerYOffset = 1.0f;

    [SerializeField]
    private float markerZOffset = -1.0f;
    //0 - Desert
    //1 - Water
    //2 - Land
    //3 - Ice
    //4 - Moon
    //5 - Sun

    // Use this for initialization
    void Start()
    { 
        markerI = marker.GetComponent<Image>();
        markerT = marker.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gs.getIsPaused() == false)
        {


            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawLine(Camera.main.transform.position, hit.point);
                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    if (hit.transform == null)
                    {
                        Debug.Log("Hit Nothing");
                        return;
                    }

                    if (hit.collider.name == "World1_Desert")
                    {
                        PlaceGUIMarker(hit);
                        markerI.sprite = markerImages[0];
                        gs.uiM.SetText(0, "Terrain Type: Desert");
                        Debug.Log("Hit Desert");
                    }
                    else if (hit.collider.name == "World1_Water")
                    {
                        PlaceGUIMarker(hit);
                        markerI.sprite = markerImages[1];
                        gs.uiM.SetText(0, "Terrain Type: Water");
                        Debug.Log("Hit Water");
                    }
                    else if (hit.collider.name == "World1_Land")
                    {
                        PlaceGUIMarker(hit);
                        markerI.sprite = markerImages[2];
                        gs.uiM.SetText(0, "Terrain Type: Land");
                        Debug.Log("Hit Land");
                    }
                    else if (hit.collider.name == "World1_Ice")
                    {
                        PlaceGUIMarker(hit);
                        markerI.sprite = markerImages[3];
                        gs.uiM.SetText(0, "Terrain Type: Ice");
                        Debug.Log("Hit Ice");
                    }
                    else if (hit.collider.name == "World1_Moon")
                    {
                        PlaceGUIMarker(hit);
                        markerI.sprite = markerImages[4];
                        gs.uiM.SetText(0, "Terrain Type: Moon");
                        Debug.Log("Hit Moon");
                    }
                    else if (hit.collider.name == "World1_Sun")
                    {
                        PlaceGUIMarker(hit);
                        markerI.sprite = markerImages[5];
                        gs.uiM.SetText(0, "Terrain Type: Sun");
                        Debug.Log("Hit Sun");
                    }
                    else
                    {
                        Debug.Log(hit.collider.name);
                    }



                }
            }

        }else
        {
            return;
        }

    }

    void PlaceGUIMarker(RaycastHit _hit)
    {
        gs.uiM.OpenMenu(1);

        markerT.position = Camera.main.WorldToScreenPoint (new Vector3(_hit.point.x, _hit.point.y + markerYOffset, _hit.point.z + markerZOffset));
       // markerT.position = new Vector3(_hit.point.x, _hit.point.y + markerYOffset, _hit.point.z + markerZOffset);
       // marker.transform.LookAt(Camera.main.transform);
        gs.setIsPaused(true);
        gs.uiM.OpenMenu(0);

    }


}
