using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour {

    public string GameName;
    public string GameVersion;
    public Canvas scrollCanvas;
    public bool offlineMode;
    private PhotonView photonView;
    public GameObject menuCam;

	void Start () {
        PhotonNetwork.offlineMode = offlineMode;
        photonView = GetComponent<PhotonView>();

        if(PlayerPrefs.HasKey("region"))
        {
            ConnectToMaster();
        }
	}

	void ConnectToMaster () {

        GetComponent<RegionSelect>().JoinRegion();
        
	}

    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("Player Disconnected?");
    }
    void OnFailedToConnectToPhoton()
    {
        Debug.Log("Failed To Connected To Photon, Internet Must Be Down");
        GetComponent<UIManager>().OpenCanvas(6);
        
    }

    void OnConnectionFail()
    {
        Debug.Log("Connection Failed");
    }

    public void SendStartGameRPC()
    {
        int seed = Random.Range(0, 100000);
        Debug.Log("Seed: " + seed.ToString());
        photonView.RPC("StartGameRPC", PhotonTargets.AllBufferedViaServer,seed);
    }

    [PunRPC]
    void StartGameRPC(int seed)
    {
        GetComponent<UIManager>().CloseAllCanvases();
        GameObject mapGenGO = GameObject.FindGameObjectWithTag("MapGen");
        mapGenGO.GetComponent<MapGenerator>().seed = seed;
        mapGenGO.GetComponent<MapGenerator>().enabled = true;
        mapGenGO.GetComponent<EndlessTerrain>().enabled = true;

        GameObject viewerObj = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity,0);
        viewerObj.name = "MyObject";
        mapGenGO.GetComponent<EndlessTerrain>().viewer = viewerObj.transform;
        viewerObj.GetComponent<ComponentsToEnable>().EnableComponents();

        menuCam.SetActive(false);
    }
}
