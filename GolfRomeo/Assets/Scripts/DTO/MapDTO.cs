using System;
using System.Xml.Serialization;
using UnityEngine;

[XmlRootAttribute("Map", Namespace = "")]
public class MapDTO : IMappingData<MapDTO, Map>
{
    //Map metadata  
    public string MapName;
    public Vector2 HeightMapSize;
    public Vector3 TextureMapSize;

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
        HeightMapSize = new Vector2(source.Terrain.terrainData.heightmapResolution, source.Terrain.terrainData.heightmapResolution);
        TextureMapSize = new Vector3(source.Terrain.terrainData.alphamapWidth, source.Terrain.terrainData.alphamapHeight, source.Terrain.terrainData.alphamapLayers);

        MapObjects = new MapObjectDTO[source.MapObjects.Length];
        Roads = new RoadDTO[source.Roads.Length];
        Checkpoints = new CheckpointDTO[source.LapTracker.Checkpoints.Length];
        Waypoints = new WaypointDTO[source.WayPointCircuit.GetComponentsInChildren<WaypointNode>().Length];

        return this;
    }

    public Map MapToGameObject(MapDTO source, Map destination = default(Map))
    {
        destination.HeightMapSize = source.HeightMapSize;
        destination.TextureMapSize = source.TextureMapSize;
        return destination;
    }
}
