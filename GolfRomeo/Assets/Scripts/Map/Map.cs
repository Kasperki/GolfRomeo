using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    public const int Road = 10;
    public const int TerrainMask = 11;
    public const int TerrainObjects = 12;

    public MapObject[] Objects;
    //LAPTIMER

    public void SaveWorld()
    {
        var WorldSerialization = new MapSerializer(this);
        Objects = GetComponentsInChildren<MapObject>();

        WorldSerialization.SaveWorld("THISISATEST.xml");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SaveWorld();
        }
    }
}
