using System.IO;
using System.Text;
using System.Xml.Serialization;

public class TrackSerializer
{
    public const string mapFileExtension = ".xml";

    private Track track;
    private TerrainSerializer terrainSerializer;

    public TrackSerializer(Track track)
    {
        this.track = track;
        terrainSerializer = new TerrainSerializer(track);
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

    public TrackDTO LoadWorld(string name)
    {
        FileStream fs = new FileStream(name + mapFileExtension, FileMode.Open, FileAccess.Read);
        XmlSerializer serializer = new XmlSerializer(typeof(TrackDTO));

        TrackDTO mapObject = (TrackDTO)serializer.Deserialize(fs);

        fs.Close();

        //Set height map
        track.Terrain.terrainData.SetHeights(0,0, terrainSerializer.DeserializeHeightMap(name, mapObject.HeightMapSize));
        track.Terrain.terrainData.SetAlphamaps(0, 0, terrainSerializer.DeserializeTextureMap(name, mapObject.TextureMapSize));

        return mapObject;
    }

    private Stream SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TrackDTO));

        TrackDTO mapDTO = new TrackDTO().MapToDTO(track);

        RoadsToDTO(mapDTO);
        MapObjectsToDTO(mapDTO);
        CheckpointsToDTO(mapDTO);
        WaypointsToDTO(mapDTO);

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        stream.Position = 0;
        return stream;
    }

    private void RoadsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < track.Roads.Length; i++)
        {
            mapDTO.Roads[i] = new RoadDTO().MapToDTO(track.Roads[i]);
            mapDTO.Roads[i].RoadNodes = new RoadNodeDTO[track.Roads[i].RoadNodes.Length];

            for (int j = 0; j < track.Roads[i].RoadNodes.Length; j++)
            {
                mapDTO.Roads[i].RoadNodes[j] = new RoadNodeDTO().MapToDTO(track.Roads[i].RoadNodes[j]);
            }
        }
    }

    private void MapObjectsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < track.MapObjects.Length; i++)
        {
            mapDTO.MapObjects[i] = new TrackObjectDTO().MapToDTO(track.MapObjects[i]);
        }
    }

    private void CheckpointsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < track.LapTracker.Checkpoints.Length; i++)
        {
            mapDTO.Checkpoints[i] = new CheckpointDTO().MapToDTO(track.LapTracker.Checkpoints[i]);
        }
    }

    private void WaypointsToDTO(TrackDTO mapDTO)
    {
        for (int i = 0; i < mapDTO.Waypoints.Length; i++)
        {
            mapDTO.Waypoints[i] = new WaypointDTO().MapToDTO(track.WayPointCircuit.GetComponentsInChildren<WaypointNode>()[i]);
        }
    }
}
