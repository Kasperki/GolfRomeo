using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CheckpointDTO : IMappingData<CheckpointDTO, Checkpoint>
{
    public int Order;
    public Vector3 Position;
    public Vector3 Rotation;

    public CheckpointDTO MapToDTO(Checkpoint source)
    {
        Order = source.CheckpointOrder;
        Position = source.transform.position;
        Rotation = source.transform.eulerAngles;

        return this;
    }

    public Checkpoint MapToGameObject(CheckpointDTO source, Checkpoint destination = default(Checkpoint))
    {
        destination.transform.position = source.Position;
        destination.transform.rotation = Quaternion.Euler(source.Rotation);
        destination.CheckpointOrder = source.Order;
        
        return destination;
    }
}
