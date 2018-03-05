using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConsoleManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject HotbarGO;
    [HideInInspector]
    public GameObject GameManagerGO;

    public Text consoleHistory;
    public InputField inputField;

    private void Start()
    {
        DebugCon.Write("Blah");
        consoleHistory.text = "[" + System.DateTime.Now.ToLocalTime() + "] Initiated Console V1.1 \n type /help for a list of commands";
    }

    public void EnterCommand(string _cmd)
    {
        inputField.text = "";
        if (_cmd.StartsWith("/"))
        {
            if (_cmd.ToLower().StartsWith("/help"))
            {
                consoleHistory.text = consoleHistory.text + "\n[" + System.DateTime.Now.ToLocalTime() + "] Commands: \n /give [itemID]  [itemAmount]";
            }
            if (_cmd.ToLower().StartsWith("/give"))
            {
                string[] cmd = _cmd.Split(" "[0]);
                if (cmd.Length == 3)
                {
                    GameManagerGO = GameObject.FindGameObjectWithTag("GM");
                    HotbarGO = GameObject.FindGameObjectWithTag("Hotbar");
                    if(int.Parse(cmd[1]) >= GameManagerGO.GetComponent<ItemIconPrefabs>().itemIcons.Length || int.Parse(cmd[1]) == 0)
                    {
                        consoleHistory.text = consoleHistory.text + "\n[" + System.DateTime.Now.ToLocalTime() + "] No Such Item ID Found";
                        return;
                    }
                    Debug.Log(int.Parse(cmd[1]));
                    ItemList.ItemType _item = GameManagerGO.GetComponent<ItemIconPrefabs>().TurnINTIntoItemType(int.Parse(cmd[1]));
                    HotbarGO.GetComponent<Inventory>().AddItem(_item, int.Parse(cmd[2]));
                    consoleHistory.text = consoleHistory.text + "\n[" + System.DateTime.Now.ToLocalTime() + "] Gave Player " + cmd[2] + " " + _item.ToString() + "'s";
                }
                else
                {
                    consoleHistory.text = consoleHistory.text + "\n[" + System.DateTime.Now.ToLocalTime() + "] Incorrect Usage, Use /give [itemID] [itemAmount]";
                }

            }
        }
        else
        {
            consoleHistory.text = consoleHistory.text + "\n[" + System.DateTime.Now.ToLocalTime() + "] Command not found, type /help for a list of commmands";
        }
    }
}
