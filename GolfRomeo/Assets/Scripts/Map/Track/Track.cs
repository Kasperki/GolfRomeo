using System;
using UnityEngine;

public class Track : Singleton<Track>
{
    public Guid ID;
    public string Name;

    public Vector2 HeightMapSize
    {
        get
        {
            return new Vector2(Terrain.terrainData.heightmapWidth, Terrain.terrainData.heightmapHeight);
        }
    }


    public Vector3 TextureMapSize
    {
        get
        {
            return new Vector3(Terrain.terrainData.alphamapWidth, Terrain.terrainData.alphamapHeight, Enum.GetNames(typeof(TerrainTextures)).Length);
        }
    }

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

    //BEHAVIOUR -------------------------------------------------
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

        //TODO META DATA
        //ID -- set track folders name + GUID....
        Name = trackName;

        //Init track objects
        InstantiateMapObjects(mapDTO);

        //Init checkpoints
        InstantiateCheckpoints(mapDTO);

        //Init waypoints
        InstantiateWaypoints(mapDTO);
    }

    private void InstantiateMapObjects(TrackDTO mapDTO)
    {
        TrackObjectsParent.transform.DestroyChildrens();

        foreach (var mapObjectDTO in mapDTO.MapObjects)
        {
            GameObject gameObj = ResourcesLoader.LoadTrackObject(mapObjectDTO.ID);
            gameObj.transform.SetParent(TrackObjectsParent.transform);

            mapObjectDTO.MapToGameObject(mapObjectDTO, gameObj.GetComponent<TrackObject>());
        }
    }

    private void InstantiateCheckpoints(TrackDTO mapDTO)
    {
        LapTracker.transform.DestroyChildrens();

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
        WayPointCircuit.transform.DestroyChildrens();

        foreach (var waypointDTO in mapDTO.Waypoints)
        {
            GameObject gameObj = ResourcesLoader.LoadRoadObject("WaypointNode");
            gameObj.transform.SetParent(WayPointCircuit.transform);

            waypointDTO.MapToGameObject(waypointDTO, gameObj.GetComponent<WaypointNode>());
        }

        if (mapDTO.Waypoints.Length > 0)
        {
            WayPointCircuit.CachePositionsAndDistances();
        }
    }
}