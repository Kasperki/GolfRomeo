using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackMetadata
{
    public Guid ID;
    public string Name;
    public float TrackRecord;

    public TrackMetadata()
    {

    }

    public TrackMetadata(string name)
    {
        Name = name;
        ID = Guid.NewGuid();
    }
}
