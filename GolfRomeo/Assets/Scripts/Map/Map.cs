using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    public string Name;
    public Vector2 MapSize;

    public const int Road = 10;
    public const int TerrainMask = 11;
    public const int TerrainObjects = 12;

    public Terrain Terrain;
    public GameObject Objects;
    public GameObject RoadsParent;

    public MapObject[] MapObjects;
    public Road[] Roads;
    //LAPTIMER

    public void SaveWorld()
    {
        var WorldSerialization = new MapSerializer(this);
        MapObjects = Objects.GetComponentsInChildren<MapObject>();
        Roads = RoadsParent.GetComponentsInChildren<Road>();

        WorldSerialization.SaveWorld("THISISATEST.xml");
    }

    public void LoadWorld()
    {
        var WorldSerialization = new MapSerializer(this);
        var map = WorldSerialization.LoadWorld("THISISATEST.xml");

        //DO STUFF WITH MAP METADATA:
        //Init Right Size map.

        //DO STUFF WITH Roads
        foreach (var tr in RoadsParent.GetComponentsInChildren<Transform>())
        {
            if (tr.GetInstanceID() == RoadsParent.transform.GetInstanceID())
            {
                continue;
            }

            Destroy(tr.gameObject);
        }

        foreach (var road in map.Roads)
        {
            GameObject roadObj = Instantiate(Resources.Load("Roads/" + road.ID, typeof(GameObject))) as GameObject;
            roadObj.transform.SetParent(RoadsParent.transform);
            road.MapToGameObject(road, roadObj.GetComponent<Road>());

            foreach (var node in road.RoadNodes)
            {
                GameObject roadNode = Instantiate(Resources.Load("Roads/Node", typeof(GameObject))) as GameObject;
                roadNode.transform.SetParent(roadObj.transform);

                node.MapToGameObject(node, roadNode.GetComponent<RoadNode>());
            }

            roadObj.GetComponent<Road>().GenerateRoadMesh();
        }

        //DO STUFF WITH MAP OBJECTS
        foreach (var tr in Objects.GetComponentsInChildren<Transform>())
        {
            if (tr.GetInstanceID() == Objects.transform.GetInstanceID())
            {
                continue;
            }

            Destroy(tr.gameObject);
        }

        foreach (var mapObjectDTO in map.MapObjects)
        {
            GameObject gameObj = Instantiate(Resources.Load("Objects/" + mapObjectDTO.ID, typeof(GameObject))) as GameObject;
            gameObj.transform.SetParent(Objects.transform);

            mapObjectDTO.MapToGameObject(mapObjectDTO, gameObj.GetComponent<MapObject>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SaveWorld();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadWorld();
        }
    }
}
