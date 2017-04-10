using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class MapSerializer
{
    private Map map;

    public MapSerializer(Map map)
    {
        this.map = map;
    }
	
    public void SaveWorld(string name)
    {
        var ms = SerializeMap(name);

        using (FileStream file = new FileStream(name, FileMode.Create, FileAccess.Write))
        {
            using (StreamReader sr = new StreamReader(ms))
            {
                var a = sr.ReadToEnd();
                byte[] toBytes = Encoding.ASCII.GetBytes(a);

                file.Write(toBytes, 0, toBytes.Length);
            }
        }
    }

    public void LoadWorld(string name)
    {
        FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read);
        XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
        DataContractSerializer ser = new DataContractSerializer(typeof(MapObject[]));

        MapObject[] worldObjects = (MapObject[])ser.ReadObject(reader, true);
        reader.Close();
        fs.Close();
    }

    private Stream SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapDTO));

        MapDTO mapDTO = new MapDTO();
        mapDTO.MapName = name;
        mapDTO.MapObjects = new MapObjectDTO[map.Objects.Length];

        //Serialize Map Objects
        for (int i = 0; i < map.Objects.Length; i++)
        {
            MapObjectDTO obj = new MapObjectDTO()
            {
                ID = map.Objects[i].ID,
                Position = map.Objects[i].Position,
                Rotation = map.Objects[i].Rotation
            };

            mapDTO.MapObjects[i] = obj;
        }

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        stream.Position = 0;
        return stream;
    }

}
