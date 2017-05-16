using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class TrackSerializer
{
    public const string mapFileExtension = ".xml";
    private Vector3 textureMapSize;

    private Track track;
    private TerrainSerializer terrainSerializer;
    private TrackFileCompressor trackCompressor;
    private DirectoryHelper directoryHelper;

    public TrackSerializer(Track track)
    {
        this.track = track;
        terrainSerializer = new TerrainSerializer(track);
        textureMapSize = new Vector3(512, 512, Enum.GetNames(typeof(TerrainTextures)).Length);

        directoryHelper = new DirectoryHelper();
        trackCompressor = new TrackFileCompressor();
    }
	
    public void SaveWorld(string name)
    {
        var trackPath = directoryHelper.GetPathToMap(name);

        using (FileStream file = new FileStream(trackPath + mapFileExtension, FileMode.Create, FileAccess.Write))
        {
            using (StreamReader sr = new StreamReader(SerializeMap(trackPath)))
            {
                var xml = sr.ReadToEnd();
                byte[] byteArray = Encoding.ASCII.GetBytes(xml);

                file.Write(byteArray, 0, byteArray.Length);
            }
        }

        terrainSerializer.Serialize(trackPath);
        trackCompressor.CreatePackage(directoryHelper.GetPathToDirectory(name)); //TODO USE TRACK MEMORY STERAM, DON*T GET THEM FROM FILES, CREATE FILES FOR DEBUG??
    }

    public TrackDTO LoadWorld(string name)
    {
        var trackPath = directoryHelper.LoadTrackPath(name);

        XmlSerializer serializer = new XmlSerializer(typeof(TrackDTO));

        TrackFileCompressor trCompressor = new TrackFileCompressor();
        TrackStreams decompressedTrackStreams = trCompressor.DecompressPackage(directoryHelper.GetPathToDirectory(name));
        TrackDTO mapObject = (TrackDTO)serializer.Deserialize(decompressedTrackStreams.TrackStream);

        track.Terrain.terrainData.SetHeights(0, 0, terrainSerializer.DeserializeHeightMap(decompressedTrackStreams.HeightMapStream, mapObject.HeightMapSize));
        track.Terrain.terrainData.SetAlphamaps(0, 0, terrainSerializer.DeserializeTextureMap(decompressedTrackStreams.TextureMapStream, textureMapSize));

        return mapObject;
    }

    private Stream SerializeMap(string name)
    {
        MemoryStream stream = new MemoryStream();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TrackDTO));

        TrackDTO mapDTO = new TrackDTO().MapToDTO(track);

        MapObjectsToDTO(mapDTO);
        CheckpointsToDTO(mapDTO);
        WaypointsToDTO(mapDTO);

        //Serialize
        xmlSerializer.Serialize(stream, mapDTO);

        stream.Position = 0;
        return stream;
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
