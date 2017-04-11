using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class MapSerializer
{
    private Map map;

    public const string mapFileExtension = ".xml";
    public const string mapHeightMapExtension = ".heightmap";

    public MapSerializer(Map map)
    {
        this.map = map;
    }
	
    public void SaveWorld(string name)
    {
        using (FileStream file = new FileStream(name + mapFileExtension, FileMode.Create, FileAccess.Write))
        {
            using (StreamReader sr = new StreamReader(SerializeMap(name)))
            {
                var xml = sr.ReadToEnd();
                byte[] byteArray = Encoding.ASCII.GetBytes(xml);

                file.Write(byteArray, 0, byteArray.Length);
            }
        }

        //Serialize Terrain
        using (FileStream file = new FileStream(name + mapHeightMapExtension, FileMode.Create, FileAccess.Write))
        {
            byte[] baData = SerializeTerrain();
            file.Write(baData, 0, baData.Length);
        }
    }

    public MapDTO LoadWorld(string name)
    {
        FileStream fs = new FileStream(name + mapFileExtension, FileMode.Open, FileAccess.Read);
        XmlSerializer serializer = new XmlSerializer(typeof(MapDTO));

        MapDTO mapObject = (MapDTO)serializer.Deserialize(fs);

        fs.Close();

        //DESERILIZE Terrain
        var heightMap = new float[(int)mapObject.MapSize.x, (int)mapObject.MapSize.y];
        FromBytes(heightMap, File.ReadAllBytes(name + mapHeightMapExtension));
        map.Terrain.terrainData.SetHeights(0, 0, heightMap);

        return mapObject;
    }

    private Stream SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapDTO));

        MapDTO mapDTO = new MapDTO().MapToDTO(map);
        mapDTO.MapObjects = new MapObjectDTO[map.MapObjects.Length];
        mapDTO.Roads = new RoadDTO[map.Roads.Length];

        //Serialize Roads
        for (int i = 0; i < map.Roads.Length; i++)
        {
            mapDTO.Roads[i] = new RoadDTO().MapToDTO(map.Roads[i]);
            mapDTO.Roads[i].RoadNodes = new RoadNodeDTO[map.Roads[i].RoadNodes.Length];

            for (int j = 0; j < map.Roads[i].RoadNodes.Length; j++)
            {
                mapDTO.Roads[i].RoadNodes[j] = new RoadNodeDTO();
                mapDTO.Roads[i].RoadNodes[j] = mapDTO.Roads[i].RoadNodes[j].MapToDTO(map.Roads[i].RoadNodes[j]);
            }
        }

        //Serialize Map Objects
        for (int i = 0; i < map.MapObjects.Length; i++)
        {
            mapDTO.MapObjects[i] = new MapObjectDTO().MapToDTO(map.MapObjects[i]);
        }

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        stream.Position = 0;
        return stream;
    }

    private byte[] SerializeTerrain()
    {
        var heightMap = map.Terrain.terrainData.GetHeights(0, 0, map.Terrain.terrainData.heightmapWidth, map.Terrain.terrainData.heightmapHeight);
        return ToBytes(heightMap);
    }

    public byte[] ToBytes<T>(T[,] array) where T : struct
    {
        var buffer = new byte[array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
        Buffer.BlockCopy(array, 0, buffer, 0, buffer.Length);
        return buffer;
    }
    public void FromBytes<T>(T[,] array, byte[] buffer) where T : struct
    {
        var len = Math.Min(array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), buffer.Length);
        Buffer.BlockCopy(buffer, 0, array, 0, len);
    }
}
