using UnityEngine;
public class UIFunctions : MonoBehaviour {

    [SerializeField]
    private UIManager uiM;

    private GamesSettingsManager gsM;
    private MapSettings mS;
    void Start()
    {
        mS = GameObject.FindGameObjectWithTag("MapSettings").GetComponent<MapSettings>();
        gsM = uiM.gs;
    }

    public void SelectLocationDialogueCancel()
    {
        gsM.setIsPaused(false);
        uiM.CloseMenu(1);
        uiM.CloseMenu(0);
    }

    public void SelectLocationDialogueStart()
    {
        Debug.Log("Loading Next Scene");
        
    }

    public void SelectLocationDialogueNaturalDisastersEnabled(bool _enabled)
    {
        Debug.Log("_enabled = " + _enabled);
        mS.disasters = _enabled;
    }

    public void SelectLocationDialogueDifficulty(int _dif)
    {
        Debug.Log("_dif = " + _dif);

        //0-EASY,1-NORMAL,2-HARD
        if(_dif == 0)
        {
            mS.difficulty = MapSettings.Difficulty.EASY;
        }
        else if(_dif == 1)
        {
            mS.difficulty = MapSettings.Difficulty.NORMAL;
        }
        else if(_dif == 2)
        {
            mS.difficulty = MapSettings.Difficulty.HARD;
        }
    }

}
