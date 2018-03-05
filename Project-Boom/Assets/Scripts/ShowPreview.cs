using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPreview : MonoBehaviour
{
    Color originalColor;
    MeshRenderer _mr;
    LevelEditor lvE = null;
    GameObject myParent;
    

    public void Load(GameObject parent,LevelEditor lv,MeshRenderer mr)
    {
        _mr = mr;
        originalColor = _mr.material.color;
        _mr.material.color = Color.black;
        lvE = lv;
        myParent = parent;
    }

    
    void Update()
    {
        if (lvE.lastSelectedPreview != myParent)
        {
            Debug.Log("Setting Colour Back To " + originalColor.ToString());
            _mr.material.color = originalColor;
            Destroy(this);
        }
    }
}
