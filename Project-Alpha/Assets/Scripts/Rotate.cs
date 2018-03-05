using UnityEngine;

public class Rotate : MonoBehaviour {
    [SerializeField]
    private GamesSettingsManager gs;

    [SerializeField]
    private float rotateSpeed = 1.0f;
    [SerializeField]
    private GameObject objectToRotate;

	// Use this for initialization
	void Start () {
		if(objectToRotate == null)
        {
            Debug.LogError("No Object To Rotate!");
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (gs.getIsPaused() == false)
        {
            objectToRotate.transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        }
        
	}
}
