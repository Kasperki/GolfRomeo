using System.IO;
using System.Text;
using System.Xml.Serialization;

public class MapSerializer
{
    public const string mapFileExtension = ".xml";

    private Map map;
    private TerrainSerializer terrainSerializer;

    public MapSerializer(Map map)
    {
        this.map = map;
        terrainSerializer = new TerrainSerializer(map);
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

        terrainSerializer.Serialize(name);
    }

    public MapDTO LoadWorld(string name)
    {
        FileStream fs = new FileStream(name + mapFileExtension, FileMode.Open, FileAccess.Read);
        XmlSerializer serializer = new XmlSerializer(typeof(MapDTO));

        MapDTO mapObject = (MapDTO)serializer.Deserialize(fs);

        fs.Close();

        //Set height map
        map.Terrain.terrainData.SetHeights(0,0, terrainSerializer.Deserialize(name, mapObject.MapSize));

        return mapObject;
    }

    private Stream SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapDTO));

        MapDTO mapDTO = new MapDTO().MapToDTO(map);

        RoadsToDTO(mapDTO);
        MapObjectsToDTO(mapDTO);
        CheckpointsToDTO(mapDTO);

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        stream.Position = 0;
        return stream;
    }

    private void RoadsToDTO(MapDTO mapDTO)
    {
        for (int i = 0; i < map.Roads.Length; i++)
        {
            mapDTO.Roads[i] = new RoadDTO().MapToDTO(map.Roads[i]);
            mapDTO.Roads[i].RoadNodes = new RoadNodeDTO[map.Roads[i].RoadNodes.Length];

            for (int j = 0; j < map.Roads[i].RoadNodes.Length; j++)
            {
                mapDTO.Roads[i].RoadNodes[j] = new RoadNodeDTO().MapToDTO(map.Roads[i].RoadNodes[j]);
            }
        }
    }

    private void MapObjectsToDTO(MapDTO mapDTO)
    {
        for (int i = 0; i < map.MapObjects.Length; i++)
        {
            mapDTO.MapObjects[i] = new MapObjectDTO().MapToDTO(map.MapObjects[i]);
        }
    }

    private void CheckpointsToDTO(MapDTO mapDTO)
    {
        for (int i = 0; i < map.LapTracker.Checkpoints.Length; i++)
        {
            mapDTO.Checkpoints[i] = new CheckpointDTO().MapToDTO(map.LapTracker.Checkpoints[i]);
        }
    }
}
