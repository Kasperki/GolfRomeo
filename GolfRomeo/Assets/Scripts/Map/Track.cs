using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : Singleton<Track>
{
    public const int RoadMask = 10;
    public const int TerrainMask = 11;
    public const int MapObjectsMask = 12;
    public const int CheckpointsMask = 13;
    public const int AIWaypointsMask = 14;

    public string Name;
    public Vector2 HeightMapSize;
    public Vector3 TextureMapSize;

    public Terrain Terrain;
    public GameObject ObjectsParent;
    public GameObject RoadsParent;
    public LapTracker LapTracker; //Checkpoints parent
    public WayPointCircuit WayPointCircuit; //Waypoints parent

    public TrackObject[] MapObjects { get { return ObjectsParent.GetComponentsInChildren<TrackObject>(); } }
    public Road[] Roads { get { return RoadsParent.GetComponentsInChildren<Road>(); } }

    public void SaveWorld()
    {
        var WorldSerialization = new TrackSerializer(this);
        WorldSerialization.SaveWorld("ThisIsATest");
    }

    public void LoadWorld(string trackName)
    {
        var WorldSerialization = new TrackSerializer(this);
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


    private void InstantiateRoads(TrackDTO mapDTO)
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

    private void InstantiateMapObjects(TrackDTO mapDTO)
    {
        ClearChilds(ObjectsParent);

        foreach (var mapObjectDTO in mapDTO.MapObjects)
        {
            GameObject gameObj = Instantiate(Resources.Load("Objects/" + mapObjectDTO.ID, typeof(GameObject))) as GameObject;
            gameObj.transform.SetParent(ObjectsParent.transform);

            mapObjectDTO.MapToGameObject(mapObjectDTO, gameObj.GetComponent<TrackObject>());
        }
    }

    private void InstantiateCheckpoints(TrackDTO mapDTO)
    {
        ClearChilds(LapTracker.gameObject);

        foreach (var checkpointDTO in mapDTO.Checkpoints)
        {
            GameObject gameObj = Instantiate(Resources.Load("Roads/Checkpoint", typeof(GameObject))) as GameObject;
            gameObj.transform.SetParent(LapTracker.transform);

            checkpointDTO.MapToGameObject(checkpointDTO, gameObj.GetComponent<Checkpoint>());
            gameObj.GetComponent<Checkpoint>().SetOrder(gameObj.GetComponent<Checkpoint>().CheckpointOrder);
        }
    }

    private void InstantiateWaypoints(TrackDTO mapDTO)
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
            LoadWorld("ThisIsATest");
        }
    }
}
