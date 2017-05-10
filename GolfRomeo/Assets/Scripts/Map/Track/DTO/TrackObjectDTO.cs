using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class TrackObjectDTO : IMappingData<TrackObjectDTO, TrackObject>
{
    public string ID;
    public Vector3 Position;
    public Vector3 Rotation;

    public TrackObjectDTO MapToDTO(TrackObject source)
    {
        ID = source.ID;
        Position = source.transform.position;
        Rotation = source.transform.eulerAngles;

        return this;
    }

    public TrackObject MapToGameObject(TrackObjectDTO source, TrackObject destination = default(TrackObject))
    {
        destination.transform.position = source.Position;
        destination.transform.rotation = Quaternion.Euler(source.Rotation);

        return destination;
    }
}