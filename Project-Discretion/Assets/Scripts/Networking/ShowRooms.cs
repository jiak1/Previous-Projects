using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShowRooms : MonoBehaviour {

    public Transform panel;
    private List<GameObject> serverList;
    private GameObject selectedObject;
    public UIManager uiM;
    private string GameName;
    private string GameVersion;
    public NetworkManager nM;
    private void Start()
    {
        GameName = nM.GameName;
        GameVersion = nM.GameVersion;
    }


    public void OnEnable()
    {
        if (serverList == null)
        {
            serverList = new List<GameObject>();
        }
        InvokeRepeating("PopulateServerList", 0, 2);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject server = EventSystem.current.currentSelectedGameObject;
            if (server != null)
            {
                if (server.name == "Server:Join")
                {
                    selectedObject = server.transform.parent.gameObject;
                }
            }
        }
    }

    public void PopulateServerList()
    {
        if(PhotonNetwork.connected == false)
        {
            JoinRegion();
        }
        if (PhotonNetwork.insideLobby == false)
        {
            PhotonNetwork.JoinLobby();
        }
        int i = 0;
        RoomInfo[] hostData = PhotonNetwork.GetRoomList();

        int selected = serverList.IndexOf(selectedObject);

        for (int j = 0; j < serverList.Count; j++)
        {
            Destroy(serverList[j]);
        }
        serverList.Clear();

        if ( hostData != null)
        {
            for (i = 0; i < hostData.Length; i++)
            {
                if (!hostData[i].IsOpen)
                    continue;

                GameObject text = (GameObject)Instantiate(Resources.Load("Server_Entry"));
                serverList.Add(text);
                text.transform.SetParent(panel, false);
                text.transform.Find("Server:Name").GetComponent<Text>().text = hostData[i].Name;
                text.transform.Find("Server:Players").GetComponent<Text>().text = hostData[i].PlayerCount + "/" + hostData[i].MaxPlayers;
                text.transform.Find("Server:Ping").GetComponent<Text>().text = hostData[i].CustomProperties["ping"].ToString();
                text.transform.Find("Server:Map").GetComponent<Text>().text = hostData[i].CustomProperties["map"].ToString();
                //text.transform.FindChild("GMText").GetComponent<Text>().text = hostData[i].CustomProperties["gm"].ToString();

            }
        }
        if (selected >= 0 && selected < serverList.Count)
        {
            selectedObject = serverList[selected];
            PhotonNetwork.JoinRoom(serverList[selected].transform.Find("Server:Name").GetComponent<Text>().text);
            uiM.OpenCanvas(4);
        }
    }


    public void JoinRegion()
    {
        if (PhotonNetwork.connectedAndReady)
        {
            PhotonNetwork.Disconnect();
        }
        string _regionCode = PlayerPrefs.GetString("region");
        if (_regionCode == "au")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.au, GameName + GameVersion);

        }
        else if (_regionCode == "asia")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.asia, GameName + GameVersion);

        }
        else if (_regionCode == "eu")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, GameName + GameVersion);

        }
        else if (_regionCode == "cae")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.cae, GameName + GameVersion);
        }
        else if (_regionCode == "jp")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.jp, GameName + GameVersion);

        }
        else if (_regionCode == "sa")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.sa, GameName + GameVersion);

        }
        else if (_regionCode == "us")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.us, GameName + GameVersion);

        }
        else if (_regionCode == "usw")
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.usw, GameName + GameVersion);

        }
    }
}
