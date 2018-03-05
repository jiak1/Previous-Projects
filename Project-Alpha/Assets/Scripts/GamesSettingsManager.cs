using UnityEngine;

public class GamesSettingsManager : MonoBehaviour
{
    [SerializeField]
    public UIManager uiM;

    private bool isPaused = false;

    public bool getIsPaused()
    {
            return isPaused;
    }

    public void setIsPaused(bool _isPaused)
    {
        isPaused = _isPaused;
        return;
        
    }

    
}
