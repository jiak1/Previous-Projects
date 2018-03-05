using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool noCanvas = false;
    private string TempServerName;
    private string TempLevelName;
    public Canvas[] canvases;
    public InputField ServerNameInput;
    public Dropdown ServerMapInput;
    //0 - Main Menu
    //1 - View Game Menu
    //2 - Create Game Menu
    //3 - Region Menu
    //4 - Waiting Menu
    //5 - SetUsername
    //6 - No Connection Menu
    //7 - Loading Menu
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label("Region: " + PlayerPrefs.GetString("region"));
    }
    // Use this for initialization
    void Start()
    {

        CloseAllCanvases();
        if (!noCanvas)
        {
            PhotonNetwork.offlineMode = false;
            if (!(PlayerPrefs.HasKey("region")))
            {
                OpenCanvas(3);
            }
            else if (!(PlayerPrefs.HasKey("username")))
            {
                OpenCanvas(5);
            }
            else
            {
                OpenCanvas(0);
                PhotonNetwork.playerName = PlayerPrefs.GetString("username");
            }
        }else
        {
            PhotonNetwork.offlineMode = true;
        }

    }
    
    // Update is called once per frame
    public void CloseAllCanvases()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].gameObject.SetActive(false);
        }
    }

    public void OpenCanvas(int id)
    {
        CloseAllCanvases();
        canvases[id].gameObject.SetActive(true);
    }

    public void ButtonPress(int id)
    {
        //Debug.Log(id);
        if (id == 1)
        {
            //Join/View Games button 
            OpenCanvas(1);
            if (PhotonNetwork.insideLobby == false)
            {
                PhotonNetwork.JoinLobby();
            }
        }
        else if (id == 2)
        {
            //Back To Main Menu Button
            OpenCanvas(0);
        }
        else if (id == 3)
        {
            //Create Game Button
            OpenCanvas(2);
        }
        else if (id == 4)
        {
            SetName();
            SetName2();

            //Create Server Button
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 5;
            string[] props = new string[] { "ping", "map" };
            roomOptions.CustomRoomPropertiesForLobby = props;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "ping", PhotonNetwork.GetPing() }, { "map", TempLevelName } };
            PhotonNetwork.CreateRoom(TempServerName, roomOptions, TypedLobby.Default);
            OpenCanvas(4);
        }
        else if (id == 5)
        {
            //Edit Region Button
            OpenCanvas(3);
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
            PlayerPrefs.DeleteKey("region");
        }
        else if (id == 6)
        {
            //Cancel Game Wait Button
            if (PhotonNetwork.isMasterClient)
            {
                foreach (PhotonPlayer _p in PhotonNetwork.otherPlayers)
                {
                    PhotonNetwork.CloseConnection(_p);
                }
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                PhotonNetwork.LeaveRoom();
            }
        }else if(id == 7)
        {
            //Retry Connection Button
            GetComponent<RegionSelect>().JoinRegion();
            OpenCanvas(7);
        }else if(id == 8)
        {
            //Start Game Button
            if (PhotonNetwork.isMasterClient)
            {
                OpenCanvas(7);
                GetComponent<NetworkManager>().SendStartGameRPC();
            }

        }

    }

    void OnJoinedRoom()
    {
        Debug.Log("Joined Room, Name: " + PhotonNetwork.room.Name + ", Players: " + PhotonNetwork.room.PlayerCount + "/" + PhotonNetwork.room.MaxPlayers + ", Ping:" + PhotonNetwork.room.CustomProperties["ping"].ToString() + ", Map:" + PhotonNetwork.room.CustomProperties["map"].ToString());
        //Debug.Log("Room Open: " + PhotonNetwork.room.IsOpen + ", Visible: " + PhotonNetwork.room.IsVisible);
    }

    void OnLeftRoom()
    {
        OpenCanvas(0);
    }

    public void SetName()
    {
        TempServerName = ServerNameInput.text;
    }

    public void SetName2()
    {
        int _id = ServerMapInput.value;
        if (_id == 0)
        {
            TempLevelName = "CE_Dustbowl";
        }
        else if (_id == 1)
        {
            TempLevelName = "BR_Sloth";
        }
        else if (_id == 2)
        {
            TempLevelName = "BS_Crap";
        }
        else
        {
            Debug.Log("No level name for ID: " + _id);
        }
    }
}
