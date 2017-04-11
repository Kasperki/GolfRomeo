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

    public MapDTO MapToDTO(Map source)
    {
        MapName = source.name;
        MapSize = new Vector2(source.Terrain.terrainData.heightmapResolution, source.Terrain.terrainData.heightmapResolution);

        return this;
    }

    public Map MapToGameObject(MapDTO source)
    {
        var Map = new Map();
        return MapToGameObject(source, Map);
    }

    public Map MapToGameObject(MapDTO source, Map destination)
    {
        destination.MapSize = source.MapSize;
        return destination;
    }
}
