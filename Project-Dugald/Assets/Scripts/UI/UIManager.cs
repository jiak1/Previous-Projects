using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public List<Canvas> canvases = new List<Canvas>();
    public List<Text> texts = new List<Text>();

    private bool consoleEnabled = false;

    //0 - Hotbar
    //1 - Console
    //2 - Loading
    //3 - Main Menu

    // Use this for initialization
    private void Awake()
    {
        DisableAllCanvases();
    }
    void Start () {
        //Enable Loading Menu
        EnableCanvas(3);
	}
    public void DisableAllCanvases()
    {
        foreach (Canvas _can in canvases)
        {
            _can.gameObject.SetActive(false);
        }
    }

    public void EnableCanvas(int _id)
    {
        canvases[_id].gameObject.SetActive(true);
    }
    public void DisableCanvas(int _id)
    {
        canvases[_id].gameObject.SetActive(false);
    }
    public void SetText(int _id, string msg)
    {
        texts[_id].text = msg;
            
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (consoleEnabled)
            {
                //Console Open Close It
                DisableCanvas(1);
                consoleEnabled = false;
            }else
            {
                if (!consoleEnabled)
                {
                    //Open Console
                    EnableCanvas(1);
                    consoleEnabled = true;
                }
            }
        }
    }

    public void GenerateWorld()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        EnableCanvas(3);
        while (canvases[3].isActiveAndEnabled == false)
        {
            yield return null;
        }
        gameObject.GetComponent<ChunkManager>().Generate();
    }

    public void QuitGame()
    {
        Application.Quit();
        
    }

}
