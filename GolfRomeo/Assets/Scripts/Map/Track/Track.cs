using AutoMapper;
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
            float length = 0;

            for (int i = 0; i < LapTracker.Checkpoints.Length; i++)
            {
                int nextNode = i + 1 == LapTracker.Checkpoints.Length ? 0 : i + 1;
                length = (LapTracker.Checkpoints[i].transform.position - LapTracker.Checkpoints[nextNode].transform.position).magnitude;
            }

            return length;
        }
    }

    //BEHAVIOUR -------------------------------------------------  //TODO -- Separate track and behaviour.
    public Terrain Terrain;
    public GameObject TrackObjectsParent;
    public LapTracker LapTracker; //Checkpoints parent
    public WayPointCircuit WayPointCircuit; //Waypoints parent
    public SkidMarks SkidMarks;

    public TrackObject[] MapObjects { get { return TrackObjectsParent.GetComponentsInChildren<TrackObject>(); } }

    private new void Awake()
    {
        base.Awake();
        SkidMarks = GetComponentInChildren<SkidMarks>();
    }

    public void NewTrack(string name)
    {
        ID = Guid.NewGuid();
        Name = name;

        var editorTools = gameObject.AddComponent<TerrainEditorTools>();
        editorTools.NewEmptyTerrain();
        Destroy(editorTools);
    }

    public void SaveTrack()
    {
        var WorldSerialization = new TrackSerializer(this);
        WorldSerialization.SaveWorld(Name);
    }

    public void LoadTrack(string trackName)
    {
        var WorldSerialization = new TrackSerializer(this);
        var mapDTO = WorldSerialization.LoadWorld(trackName);
        SkidMarks.Init();

        //MAP METADATA
        Mapper.Map(mapDTO, this);

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

            Mapper.Map(mapObjectDTO, gameObj.GetComponent<TrackObject>());
        }
    }

    private void InstantiateCheckpoints(TrackDTO mapDTO)
    {
        LapTracker.transform.DestroyChildrens();

        foreach (var checkpointDTO in mapDTO.Checkpoints)
        {
            GameObject gameObj = ResourcesLoader.LoadRoadObject("Checkpoint");
            gameObj.transform.SetParent(LapTracker.transform);

            Mapper.Map(checkpointDTO, gameObj.GetComponent<Checkpoint>());
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

            Mapper.Map(waypointDTO, gameObj.GetComponent<WaypointNode>());
        }

        if (mapDTO.Waypoints.Length > 0)
        {
            WayPointCircuit.CachePositionsAndDistances();
        }
    }
}