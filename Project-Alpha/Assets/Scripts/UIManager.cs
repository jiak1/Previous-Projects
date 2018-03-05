using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class UIManager : MonoBehaviour {


    public GamesSettingsManager gs;
    public Canvas[] Menus;
    //0-SphereSelectionLocation
    //1-MarkerCanvas
    public Text[] Texts;
    //0-TerrainType
    //1-
    public Button[] Buttons;
    public Slider[] Sliders;
    


	void Awake () {
        CloseAllMenus();
	}
	


    public void CloseMenu(int _id)
    {
        Menus[_id].enabled = false;
        return;
    }

    public void CloseAllMenus()
    {
        foreach (Canvas _c in Menus)
        {
            _c.enabled = false;
        }
    }

    public void OpenMenu(int _id)
    {
        Menus[_id].enabled = true;

    }

    public void SetText(int _id, string _msg)
    {
        Texts[_id].text = _msg;
    }
}
