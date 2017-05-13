using System;
using UnityEngine;

public class Track : Singleton<Track>
{
    public Guid ID;
    public string Name;
    public Vector2 HeightMapSize;
    public Vector3 TextureMapSize;

    public float TrackLenghtInKilometers
    {
        get
        {
            float lenght = 0;

            for (int i = 0; i < LapTracker.Checkpoints.Length; i++)
            {
                int nextNode = i + 1 == LapTracker.Checkpoints.Length ? 0 : i + 1;
                lenght = (LapTracker.Checkpoints[i].transform.position - LapTracker.Checkpoints[nextNode].transform.position).magnitude;
            }

            return lenght;
        }
    }

    //Track objects
    public Terrain Terrain;
    public GameObject TrackObjectsParent;
    public LapTracker LapTracker; //Checkpoints parent
    public WayPointCircuit WayPointCircuit; //Waypoints parent

    public TrackObject[] MapObjects { get { return TrackObjectsParent.GetComponentsInChildren<TrackObject>(); } }

    public void SaveTrack()
    {
        var WorldSerialization = new TrackSerializer(this);
        WorldSerialization.SaveWorld(Name);
    }

    public void LoadTrack(string trackName)
    {
        var WorldSerialization = new TrackSerializer(this);
        var mapDTO = WorldSerialization.LoadWorld(trackName);

        //Init map objects
        InstantiateMapObjects(mapDTO);

        //Init checkpoints
        InstantiateCheckpoints(mapDTO);

        //Init waypoints
        InstantiateWaypoints(mapDTO);
    }

    private void InstantiateMapObjects(TrackDTO mapDTO)
    {
        ClearChilds(TrackObjectsParent);

        foreach (var mapObjectDTO in mapDTO.MapObjects)
        {
            GameObject gameObj = ResourcesLoader.LoadTrackObject(mapObjectDTO.ID);
            gameObj.transform.SetParent(TrackObjectsParent.transform);

            mapObjectDTO.MapToGameObject(mapObjectDTO, gameObj.GetComponent<TrackObject>());
        }
    }

    private void InstantiateCheckpoints(TrackDTO mapDTO)
    {
        ClearChilds(LapTracker.gameObject);

        foreach (var checkpointDTO in mapDTO.Checkpoints)
        {
            GameObject gameObj = ResourcesLoader.LoadRoadObject("Checkpoint");
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
            GameObject gameObj = ResourcesLoader.LoadRoadObject("WaypointNode");
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
            SaveTrack();
        }
    }
}