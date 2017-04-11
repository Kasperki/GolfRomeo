using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoadNodeDTO : IMappingData<RoadNodeDTO, RoadNode>
{
    public Vector3 Position;
    public Vector3 Rotation;

    public RoadNodeDTO MapToDTO(RoadNode source)
    {
        Position = source.transform.position;
        Rotation = source.transform.eulerAngles;

        return this;
    }

    public RoadNode MapToGameObject(RoadNodeDTO source)
    {
        var roadNode = new RoadNode();
        return MapToGameObject(source, roadNode);
    }

    public RoadNode MapToGameObject(RoadNodeDTO source, RoadNode destination)
    {
        destination.transform.position = source.Position;
        destination.transform.rotation = Quaternion.Euler(source.Rotation);

        return destination;
    }
}
