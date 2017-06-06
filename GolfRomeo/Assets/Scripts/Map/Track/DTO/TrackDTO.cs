using System;
using System.Xml.Serialization;
using UnityEngine;

[XmlRootAttribute("Track", Namespace = "")]
public class TrackDTO
{
    public Guid ID;
    public string MapName;
    public float TrackRecord;

    /// <summary>
    /// Track Objects - trees etc.
    /// </summary>
    public TrackObjectDTO[] MapObjects;

    /// <summary>
    /// Track checkpoints
    /// </summary>
    public CheckpointDTO[] Checkpoints;

    /// <summary>
    /// Track checkpoints
    /// </summary>
    public WaypointDTO[] Waypoints;
}
