using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class MapObjectDTO : IMappingData<MapObjectDTO, MapObject>
{
    public string ID;
    public Vector3 Position;
    public Vector3 Rotation;

    public MapObjectDTO MapToDTO(MapObject source)
    {
        ID = source.ID;
        Position = source.transform.position;
        Rotation = source.transform.eulerAngles;

        return this;
    }

    public MapObject MapToGameObject(MapObjectDTO source)
    {
        var MapObject = new MapObject();
        return MapToGameObject(source, MapObject);
    }

    public MapObject MapToGameObject(MapObjectDTO source, MapObject destination)
    {
        destination.transform.position = source.Position;
        destination.transform.rotation = Quaternion.Euler(source.Rotation);

        return destination;
    }
}