using System;
using UnityEngine;

public class Track : Singleton<Track>
{
    public Guid ID;
    public string Name;
    public float TrackRecord;

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

    public Terrain Terrain;
    public GameObject TrackObjectsParent;
    public LapTracker LapTracker;
    public WayPointCircuit WayPointCircuit;
    public SkidMarks SkidMarks;

    public TrackObject[] MapObjects { get { return TrackObjectsParent.GetComponentsInChildren<TrackObject>(); } }

    private new void Awake()
    {
        base.Awake();
        SkidMarks = GetComponentInChildren<SkidMarks>();
    }
}