using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaypointDTO : IMappingData<WaypointDTO, WaypointNode>
{
    public Vector3 Position;

    public WaypointDTO MapToDTO(WaypointNode source)
    {
        Position = source.transform.position;
        return this;
    }

    public WaypointNode MapToGameObject(WaypointDTO source, WaypointNode destination = default(WaypointNode))
    {
        destination.transform.position = source.Position;
        return destination;
    }
}
