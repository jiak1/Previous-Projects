using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugCon : MonoBehaviour
{

    public static void Write(string input)
    {
        bool DoDebugLog = true;
        if (DoDebugLog)
        {
            Debug.Log(input);
        }
    }
}

