using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    public const int RoadMask = 10;
    public const int TerrainMask = 11;
    public const int MapObjectsMask = 12;
    public const int CheckpointsMask = 13;
    public const int AIWaypointsMask = 14;

    public string Name;
    public Vector2 MapSize;

    public Terrain Terrain;
    public GameObject ObjectsParent;
    public GameObject RoadsParent;
    public GameObject CheckpointsParent;

    public MapObject[] MapObjects { get { return ObjectsParent.GetComponentsInChildren<MapObject>(); } }
    public Road[] Roads { get { return RoadsParent.GetComponentsInChildren<Road>(); } }

    public LapTracker LapTracker;
    public WayPointCircuit WayPointCircuit;

    public void SaveWorld()
    {
        var WorldSerialization = new MapSerializer(this);
        WorldSerialization.SaveWorld("ThisIsATest");
    }

    public void LoadWorld()
    {
        var WorldSerialization = new MapSerializer(this);
        var mapDTO = WorldSerialization.LoadWorld("ThisIsATest");

        //TODO DO STUFF WITH METADATA.

        //Init roads
        InstantiateRoads(mapDTO);

        //Init map objects
        InstantiateMapObjects(mapDTO);

        //Init checkpoints
        InstantiateCheckpoints(mapDTO);

        //Init waypoints
        InstantiateWaypoints(mapDTO);
    }


    private void InstantiateRoads(MapDTO mapDTO)
    {
        ClearChilds(RoadsParent);

        foreach (var road in mapDTO.Roads)
        {
            GameObject roadObj = Instantiate(Resources.Load("Roads/" + road.ID, typeof(GameObject))) as GameObject;
            roadObj.transform.SetParent(RoadsParent.transform);
            road.MapToGameObject(road, roadObj.GetComponent<Road>());

            foreach (var node in road.RoadNodes)
            {
                GameObject roadNode = Instantiate(Resources.Load("Roads/RoadNode", typeof(GameObject))) as GameObject;
                roadNode.transform.SetParent(roadObj.transform);

                node.MapToGameObject(node, roadNode.GetComponent<RoadNode>());
            }

            roadObj.GetComponent<Road>().GenerateRoadMesh();
        }
    }

    private void InstantiateMapObjects(MapDTO mapDTO)
    {
        ClearChilds(ObjectsParent);

        foreach (var mapObjectDTO in mapDTO.MapObjects)
        {
            GameObject gameObj = Instantiate(Resources.Load("Objects/" + mapObjectDTO.ID, typeof(GameObject))) as GameObject;
            gameObj.transform.SetParent(ObjectsParent.transform);

            mapObjectDTO.MapToGameObject(mapObjectDTO, gameObj.GetComponent<MapObject>());
        }
    }

    private void InstantiateCheckpoints(MapDTO mapDTO)
    {
        ClearChilds(CheckpointsParent);

        foreach (var checkpointDTO in mapDTO.Checkpoints)
        {
            GameObject gameObj = Instantiate(Resources.Load("Roads/Checkpoint", typeof(GameObject))) as GameObject;
            gameObj.transform.SetParent(CheckpointsParent.transform);

            checkpointDTO.MapToGameObject(checkpointDTO, gameObj.GetComponent<Checkpoint>());
            gameObj.GetComponent<Checkpoint>().SetOrder(gameObj.GetComponent<Checkpoint>().CheckpointOrder);
        }
    }

    private void InstantiateWaypoints(MapDTO mapDTO)
    {
        ClearChilds(WayPointCircuit.gameObject);

        foreach (var waypointDTO in mapDTO.Waypoints)
        {
            GameObject gameObj = Instantiate(Resources.Load("Roads/WaypointNode", typeof(GameObject))) as GameObject;
            gameObj.transform.SetParent(WayPointCircuit.transform);

            waypointDTO.MapToGameObject(waypointDTO, gameObj.GetComponent<WaypointNode>());
        }

        WayPointCircuit.CachePositionsAndDistances();
    }

    private void ClearChilds(GameObject obj)
    {
        foreach (var tr in obj.GetComponentsInChildren<Transform>())
        {
            if (tr.GetInstanceID() == obj.transform.GetInstanceID())
            {
                continue;
            }

            Destroy(tr.gameObject);
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
