using BLINDED_AM_ME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoadDTO : IMappingData<RoadDTO, Road>
{
    public string ID;
    public bool IsCircuit;
    public RoadNodeDTO[] RoadNodes;

    public RoadDTO MapToDTO(Road source)
    {
        ID = source.ID;
        IsCircuit = source.IsCircuit;

        return this;
    }

    public Road MapToGameObject(RoadDTO source, Road destination = default(Road))
    {
        destination.gameObject.GetComponent<Path_Comp>().isCircuit = source.IsCircuit;
        return destination;
    }
}
