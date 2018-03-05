using UnityEngine;
using System.Collections.Generic;
public class PlacingManager : MonoBehaviour {

    [SerializeField]
    private GamesSettingsManager gs;

    private RaycastHit hit;
    [SerializeField]
    private Transform placingArea;
    [SerializeField]
    private float cellWidth;
    private GameObject fakeBuilding;

    List<Vector3> placedBuildings = new List<Vector3>();

    private Vector3 currentLocation;
    [SerializeField]
    private GameObject fakeBuildingPrefab;
    [SerializeField]
    private GameObject buildingPrefab;
    [SerializeField]
    private GameObject tiles;
    // Use this for initialization
    void Start () {
        Debug.Log("Initialising Placing Manager");
        if(placingArea == null)
        {
            Debug.LogError("Plane Transform Not Set!");
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
           
            Vector3 pivotToPoint = hit.point - placingArea.transform.position;
            float tileZ = pivotToPoint.z / cellWidth;
            tileZ = Mathf.Round(tileZ);
            float worldZ = placingArea.transform.position.z + tileZ * cellWidth;

            float tileX = pivotToPoint.x / cellWidth;
            tileX = Mathf.Round(tileX);
            float worldX = placingArea.transform.position.x + tileX * cellWidth;

            if (fakeBuilding != null)
            {
                Destroy(fakeBuilding);
            }
            Vector3 instantiateLocation = new Vector3(worldX, 0, worldZ);
            fakeBuilding = Instantiate(fakeBuildingPrefab, instantiateLocation, Quaternion.identity);
            fakeBuilding.transform.SetParent(this.transform);
            currentLocation = instantiateLocation;
           
                


        } else
        {
            Debug.Log("Hit nothing");
            if (fakeBuilding != null)
            {
                Destroy(fakeBuilding);
            }

        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Mouse Button Down");

            if (placedBuildings.Count == 0)
            {
                Debug.Log("Running For Loop");
                for (int i = 0; i < placedBuildings.Count; i++)
                {
                    if (currentLocation.ToString() == placedBuildings[i].ToString())
                    {
                        Debug.Log("Not placing because building already there!");
                        return;
                    }
                }
                

                Debug.Log("Placing Building");
                GameObject placedBuilding = Instantiate(buildingPrefab, currentLocation, Quaternion.identity);
                placedBuildings.Add(placedBuilding.transform.position);
                placedBuilding.transform.SetParent(tiles.transform);

            }
            else
            {
                Debug.Log("Placing Building");
                GameObject placedBuilding = Instantiate(buildingPrefab, currentLocation, Quaternion.identity);
                placedBuildings.Add(placedBuilding.transform.position);
                placedBuilding.transform.SetParent(tiles.transform);
            }
        }
    }


   

}
