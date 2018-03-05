using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class FileUtil
{


    public static void SaveGame(string name, List<GameObject> _mapData)
    {

        if (!(Directory.Exists(Application.persistentDataPath + "/Saves/" + name)))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/" + name);
        }

        MapData _myMap = new MapData();

        foreach (GameObject _go in _mapData)
        {
            if (_go != null)
            {
                GOData _goD = _go.GetComponent<GOData>();
                BlockEntry bE = new BlockEntry();
                bE.blockName = _goD.blockTypeName;
                bE.blockPosition = _goD.position;
                bE.blockYRotation = _goD.yRotation;
                bE.coloured = _goD.coloured;
                _myMap.mapBlocks.Add(bE);
            }
        }

        XmlSerializer serialiser = new XmlSerializer(typeof(MapData));
        FileStream stream = new FileStream(Application.persistentDataPath + "/Saves/" + name + "/block_data.xml", FileMode.Create);
        serialiser.Serialize(stream, _myMap);
        stream.Close();
        Debug.Log("Successfully Saved Level: " + name + " At " + Application.persistentDataPath + "/Saves/" + name + "/block_data.xml");
    }

    public static List<string> GetAllSaves()
    {
        List<string> saves = new List<string>();
        saves.Clear();
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/Saves");
        DirectoryInfo[] dirInfo = info.GetDirectories();

        foreach (DirectoryInfo dI in dirInfo)
        {
            saves.Add(dI.Name);
        }
        return saves;
    }

    public static void DeleteSaveByName(string saveName)
    {
        if (!(Directory.Exists(Application.persistentDataPath + "/Saves/" + saveName + "/")))
        {
            Debug.LogError("NO SAVE DIRECTORY EXISTS!!!!!");

        }
        else
        {

            Directory.Delete(Application.persistentDataPath + "/Saves/" + saveName + "/",true);
        }
    }

    public static MapData loadSave(string levelName)
    {
        
        if (!(Directory.Exists(Application.persistentDataPath + "/Saves/" + levelName + "/")))
        {
            Debug.LogError("NO SAVE DIRECTORY EXISTS!!!!!");
            return null;
        }
        else
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(MapData));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Saves/" + levelName + "/block_data.xml", FileMode.Open);

            MapData md = (MapData) serialiser.Deserialize(stream);
            stream.Close();
            return md;
        }
    }
}

[System.Serializable]
public class MapData
{
    public List<BlockEntry> mapBlocks = new List<BlockEntry>();
}
[System.Serializable]
public class BlockEntry
{
    public string blockName;
    public Vector3 blockPosition;
    public float blockYRotation;
    public bool coloured;
}