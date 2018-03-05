using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyStats : MonoBehaviour {

    public Text _players;
    public Text _sName;
    public Text _sMap;

    public void OnEnable()
    {

        StartCoroutine(wait2());
    }

    IEnumerator wait2()
    {
        yield return new WaitForSeconds(2);
        PopulateStats();
    }


    void PopulateStats()
    {
        //Debug.Log("Room Open: " + PhotonNetwork.room.IsOpen + ", Visible: " + PhotonNetwork.room.IsVisible);
        _sName.text = "Server Name: " + PhotonNetwork.room.Name;
        _sMap.text = "Map: " + PhotonNetwork.room.CustomProperties["map"].ToString();

        string players = " - " + PhotonNetwork.playerName;
        foreach (PhotonPlayer _p in PhotonNetwork.otherPlayers)
        {
            players = players + "\n - " + _p.NickName;
        }
        _players.text = players;
        StartCoroutine(wait2());
    }

    void OnReceivedRoomListUpdate()
    {
        PopulateStats();
    }

    void OnReceivedRoomList()
    {
        PopulateStats();
    }
}
