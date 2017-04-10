using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRootAttribute("Map", Namespace = "")]
public class MapDTO
{
    //Map metadata  
    public string MapName;

    //public MapSize
    //public Terrain terrainData;

    public MapObjectDTO[] MapObjects;

}
