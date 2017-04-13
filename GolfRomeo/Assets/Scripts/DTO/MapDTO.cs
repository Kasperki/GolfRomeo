using System;
using System.Xml.Serialization;
using UnityEngine;

[XmlRootAttribute("Map", Namespace = "")]
public class MapDTO : IMappingData<MapDTO, Map>
{
    //Map metadata  
    public string MapName;
    public Vector2 MapSize;

    /// <summary>
    /// Map Objects - trees etc.
    /// </summary>
    public MapObjectDTO[] MapObjects;

    /// <summary>
    /// Map all roads
    /// </summary>
    public RoadDTO[] Roads;

    /// <summary>
    /// Map checkpoints
    /// </summary>
    public CheckpointDTO[] Checkpoints;

    /// <summary>
    /// Map checkpoints
    /// </summary>
    public WaypointDTO[] Waypoints;

    public MapDTO MapToDTO(Map source)
    {
        MapName = source.name;
        MapSize = new Vector2(source.Terrain.terrainData.heightmapResolution, source.Terrain.terrainData.heightmapResolution);

        MapObjects = new MapObjectDTO[source.MapObjects.Length];
        Roads = new RoadDTO[source.Roads.Length];
        Checkpoints = new CheckpointDTO[source.LapTracker.Checkpoints.Length];
        Waypoints = new WaypointDTO[source.WayPointCircuit.GetComponentsInChildren<WaypointNode>().Length];

        return this;
    }

    public Map MapToGameObject(MapDTO source, Map destination = default(Map))
    {
        destination.MapSize = source.MapSize;
        return destination;
    }
}
