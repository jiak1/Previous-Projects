using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RegionSelect : MonoBehaviour
{

    public Text regionTxt;
    public Text regionPingTxt;

    private bool pressedbutton = false;
    private string GameName;
    private string GameVersion;

    private string currentRegion = "NONE";

    public RegionStat[] regionStats;


    private void Start()
    {
        GameName = GetComponent<NetworkManager>().GameName;
        GameVersion = GetComponent<NetworkManager>().GameVersion;
        if (!(PlayerPrefs.HasKey("region")))
        {
            pressedbutton = false;
        }
        else
        {
            pressedbutton = true;
        }
    }

    private void Update()
    {
        if (!(PlayerPrefs.HasKey("region")))
        {
            pressedbutton = false;
        }
    }

    public void RegionChanged(string _regionCode)
    {
        currentRegion = _regionCode;
        if (_regionCode == "au")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.au, GameName + GameVersion);
            regionTxt.text = "Region Selected: Australia";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            // Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());
            PlayerPrefs.SetString("region", "au");

        }
        else if (_regionCode == "asia")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.asia, GameName + GameVersion);
            regionTxt.text = "Region Selected: Asia";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "asia");
            // Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());

        }
        else if (_regionCode == "eu")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, GameName + GameVersion);
            regionTxt.text = "Region Selected: Europe";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "eu");
            //Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());

        }
        else if (_regionCode == "cae")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.cae, GameName + GameVersion);
            regionTxt.text = "Region Selected: Canada";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "cae");
            //Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());

        }
        else if (_regionCode == "jp")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.jp, GameName + GameVersion);
            regionTxt.text = "Region Selected: Japan";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "jp");
            // Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());

        }
        else if (_regionCode == "sa")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.sa, GameName + GameVersion);
            regionTxt.text = "Region Selected: South America";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "sa");
            //  Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());

        }
        else if (_regionCode == "us")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.us, GameName + GameVersion);
            regionTxt.text = "Region Selected: U.S.A East";
            //regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "us");
            // Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());

        }
        else if (_regionCode == "usw")
        {
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.ConnectToRegion(CloudRegionCode.usw, GameName + GameVersion);
            regionTxt.text = "Region Selected: U.S.A West";
            // regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
            PlayerPrefs.SetString("region", "usw");
            //Debug.Log("Ping: " + PhotonNetwork.GetPing().ToString());


        }
        PlayerPrefs.Save();
    }
    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
    }
    public void SelectRegionButtonPressed()
    {
        JoinRegion();
        if (!(PlayerPrefs.HasKey("username")))
        {
            GetComponent<UIManager>().OpenCanvas(5);
        }
        else
        {
            if (PhotonNetwork.connected) { PhotonNetwork.Disconnect(); }
            GetComponent<UIManager>().OpenCanvas(0);

        }
        PlayerPrefs.Save();
        pressedbutton = true;
    }

    void OnJoinedLobby()
    {
        for (int i = 0; i < regionStats.Length; i++)
        {
            RegionStat region = regionStats[i];
            if (region.regionCode == currentRegion)
            {
                Debug.Log("REgion equal");
                region.ping = PhotonNetwork.GetPing().ToString();
                int myPing = PhotonNetwork.GetPing();
                if(myPing >= 150) { region.button.colors.normalColor.Equals(Color.red); }else if(myPing >= 65) { region.button.colors.normalColor.Equals(Color.yellow); }else if(myPing >= 1) { region.button.colors.normalColor.Equals(Color.green); }else { region.button.colors.normalColor.Equals(Color.black); }
            }
        }
        regionPingTxt.text = "Ping: " + PhotonNetwork.GetPing().ToString();
        if (pressedbutton == false)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public void JoinRegion()
    {
        if (!(PlayerPrefs.HasKey("region")))
        {
            GetComponent<UIManager>().OpenCanvas(3);
        }
        else
        {
            if (PhotonNetwork.connectedAndReady)
            {
                PhotonNetwork.Disconnect();
            }
            string _regionCode = PlayerPrefs.GetString("region");
            if (_regionCode == "au")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.au, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.au);
            }
            else if (_regionCode == "asia")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.asia, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.au);
            }
            else if (_regionCode == "eu")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.eu);
            }
            else if (_regionCode == "cae")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.cae, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.cae);
            }
            else if (_regionCode == "jp")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.jp, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.jp);
            }
            else if (_regionCode == "sa")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.sa, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.sa);
            }
            else if (_regionCode == "us")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.us, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.us);
            }
            else if (_regionCode == "usw")
            {
                PhotonNetwork.ConnectToRegion(CloudRegionCode.usw, GameName + GameVersion);
                PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.usw);
            }
        }
    }
}
[System.Serializable]
public struct RegionStat
{
    public string regionCode;
    public string ping;
    public Button button;
}
