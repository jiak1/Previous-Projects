using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSettings : MonoBehaviour {


    public string Map_Name = "Map001";
    public bool disasters;
    public enum Difficulty { EASY, NORMAL, HARD };

    public Difficulty difficulty;


    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
