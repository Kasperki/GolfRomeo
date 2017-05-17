using System;
using System.Xml.Serialization;
using UnityEngine;

[XmlRootAttribute("Track", Namespace = "")]
public class TrackDTO : IMappingData<TrackDTO, Track>
{
    //Map metadata  
    public string MapName;
    public Vector2 HeightMapSize;
    public Vector3 TextureMapSize;

    /// <summary>
    /// Map Objects - trees etc.
    /// </summary>
    public TrackObjectDTO[] MapObjects;

    /// <summary>
    /// Map checkpoints
    /// </summary>
    public CheckpointDTO[] Checkpoints;

    /// <summary>
    /// Map checkpoints
    /// </summary>
    public WaypointDTO[] Waypoints;

    public TrackDTO MapToDTO(Track source)
    {
        MapName = source.name;
        HeightMapSize = new Vector2(source.Terrain.terrainData.heightmapResolution, source.Terrain.terrainData.heightmapResolution);
        TextureMapSize = new Vector3(source.Terrain.terrainData.alphamapWidth, source.Terrain.terrainData.alphamapHeight, source.Terrain.terrainData.alphamapLayers);

        MapObjects = new TrackObjectDTO[source.MapObjects.Length];
        Checkpoints = new CheckpointDTO[source.LapTracker.Checkpoints.Length];
        Waypoints = new WaypointDTO[source.WayPointCircuit.GetComponentsInChildren<WaypointNode>().Length];

        return this;
    }

    public Track MapToGameObject(TrackDTO source, Track destination = default(Track))
    {
        return destination;
    }
}
