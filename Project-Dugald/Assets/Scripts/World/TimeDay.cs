using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDay : MonoBehaviour {
	
    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    public float duration = 3.0F;
    private Camera cam;
	public int daysPassed;
	public int GrowthTicks;
    void Start()
    {
        cam = Camera.main;

    }

    void Update()
    {

        float t = Mathf.PingPong(Time.time, duration) / duration;
        cam.backgroundColor = Color.Lerp(color1, color2, t);

		if (t == duration) {daysPassed++;}
		if (t == duration || t == duration / 2 || t == duration / 4 || t == t * 0.75)
		{GrowthTicks++;}
 
		}
}
