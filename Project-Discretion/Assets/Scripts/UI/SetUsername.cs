using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetUsername : MonoBehaviour {

    public InputField input;
    public UIManager uiM;

    public void ButtonSumbit()
    {
        PlayerPrefs.SetString("username", input.text);
        PhotonNetwork.playerName = input.text;
        uiM.OpenCanvas(0);
    }

}
